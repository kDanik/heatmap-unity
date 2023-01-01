using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

/// <summary>
/// Controls interactions with particle system of heatmap
/// </summary>
public class HeatmapParticleSystem
{
    private ParticleSystem particleSystem;

    /// <summary>
    /// This array is NOT representing directly real particles of particle system. It serves as buffer for particles.
    /// Particles in ParticleSystem can't be modified directly, so each change should be applied using particleSystem.SetParticles().
    /// </summary>
    private Particle[,,] particles;

    private Bounds particleSystemBounds;
    private Vector3Int sizeInParticles;

    /// <summary>
    /// Creates and configures particle system for heatmap visualisation.
    /// </summary>
    /// <param name="parent">Object that will contain ParticleSystem component</param>
    public void InitializeParticleSystem(GameObject parent, HeatmapController.Settings settings)
    {
        particleSystemBounds = parent.GetComponent<BoxCollider>().bounds;
        particleSystem = CreateAndConfigureParticleSystem(parent, settings);
    }

    /// <summary>
    /// Creates and populates particle array
    /// </summary>
    public void CreateParticleArray(HeatmapController.Settings settings)
    {
        sizeInParticles = CalculateSizeOfParticleSystemInParticles(settings);

        particles = new Particle[sizeInParticles.x, sizeInParticles.y, sizeInParticles.z];

        for (int x = 0; x < sizeInParticles.x; x += 1)
        {
            for (int y = 0; y < sizeInParticles.y; y += 1)
            {
                for (int z = 0; z < sizeInParticles.z; z += 1)
                {
                    Particle particle = new();

                    Vector3 position = ConvertParticleGridPositionToGlobal(new Vector3Int(x, y, z), settings);
                    particle.position = position;

                    particle.startSize = settings.particleSize;
                    particle.startColor = settings.gradient.Evaluate(0);

                    particle.remainingLifetime = 1000;
                    particle.startLifetime = 1000;

                    particles[x, y, z] = particle;
                }
            }
        }
    }

    /// <summary>
    /// Checks particles and adds its particles to particle system.
    /// Depending  on colorCutoff and their alpha, particles will be added or ignored.
    /// </summary>
    public void UpdateParticlesInParticleSystem(float[,,] particleColorValues, HeatmapController.Settings settings)
    {
        List<Particle> particleList = new();

        for (int x = 0; x < sizeInParticles.x; x += 1)
        {
            for (int y = 0; y < sizeInParticles.y; y += 1)
            {
                for (int z = 0; z < sizeInParticles.z; z += 1)
                {
                    // the invisible particles / particles with color value lower than colorCutoff should NOT be added to particle system
                    if (settings.colorCutoff <= particleColorValues[x, y, z])
                    {
                        Color particleColor = settings.gradient.Evaluate(particleColorValues[x, y, z]);
                        if (particleColor.a > 0.001f)
                        {
                            particles[x, y, z].startColor = particleColor;
                            particleList.Add(particles[x, y, z]);
                        }
                    }
                }
            }
        }

        particleSystem.SetParticles(particleList.ToArray());
    }


    /// <summary>
    /// Calculates size (bounds) of particle system in particles
    /// </summary>
    private Vector3Int CalculateSizeOfParticleSystemInParticles(HeatmapController.Settings settings)
    {
        Vector3Int calculatedSizeInParticles = Vector3Int.FloorToInt((particleSystemBounds.max - particleSystemBounds.min) / settings.particleDistance);

        if (settings.heightInParticles != 0)
        {
            calculatedSizeInParticles.y = settings.heightInParticles;
        }

        return calculatedSizeInParticles;
    }

    /// <summary>
    /// Converts global position to closest index in particle grid (particles array)
    /// </summary>
    public Vector3Int ConvertGlobalPositionToParticleGrid(Vector3 globalPosition, HeatmapController.Settings settings)
    {
        Vector3Int convertedPosition = Vector3Int.RoundToInt((globalPosition - particleSystemBounds.min) / settings.particleDistance);

        return convertedPosition;
    }

    /// <summary>
    /// Converts position in particles array to global position
    /// </summary>
    public Vector3 ConvertParticleGridPositionToGlobal(Vector3Int positionInParticleGrid, HeatmapController.Settings settings)
    {
        Vector3 convertedPosition;
        convertedPosition.x = (positionInParticleGrid.x * settings.particleDistance) + particleSystemBounds.min.x;
        convertedPosition.y = (positionInParticleGrid.y * settings.particleDistance) + particleSystemBounds.min.y;
        convertedPosition.z = (positionInParticleGrid.z * settings.particleDistance) + particleSystemBounds.min.z;

        return convertedPosition;
    }

    public Vector3Int GetSizeOfParticleSystemInParticles()
    {
        return sizeInParticles;
    }

    private ParticleSystem CreateAndConfigureParticleSystem(GameObject parent, HeatmapController.Settings settings)
    {
        ParticleSystem newParticleSystem = parent.AddComponent<ParticleSystem>();

        EmissionModule emission = newParticleSystem.emission;
        emission.enabled = false;

        ShapeModule shape = newParticleSystem.shape;
        shape.enabled = false;

        ParticleSystemRenderer renderer = parent.GetComponent<ParticleSystemRenderer>();
        renderer.sortMode = ParticleSystemSortMode.Distance;
        renderer.allowRoll = false;
        renderer.alignment = ParticleSystemRenderSpace.Facing;

        MainModule main = newParticleSystem.main;
        main.loop = false;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = settings.maxParticleNumber;
        main.playOnAwake = false;


        renderer.material = settings.particleMaterial;

        return newParticleSystem;
    }
}

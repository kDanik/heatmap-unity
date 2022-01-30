using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public static class HeatmapVisualisation
{
    private static ParticleSystem particleSystem;

    private static GameObject parentObject;
    private static BoxCollider parentCollider;
    private static HeatmapController.Settings settings;

    private static ParticleSystem.Particle [,,] particles;
    private static Vector3Int size;

    private static float[,,] color;


    public static ParticleSystem InitParticleSystem(GameObject parent)
    {
        if (parent.GetComponent<ParticleSystem>() == null)
        {
            //Getting references
            parentObject = parent;
            parentCollider = parent.GetComponent<BoxCollider>();
            settings = parent.GetComponent<HeatmapController>().settings;

            //creating particle system on parent object
            particleSystem = new ParticleSystem();
            particleSystem = parent.AddComponent<ParticleSystem>();

            var emission = particleSystem.emission;
            emission.enabled = false;

            var shape = particleSystem.shape;
            shape.enabled = false;

            var renderer = parent.GetComponent<ParticleSystemRenderer>();
            renderer.sortMode = ParticleSystemSortMode.Distance;
            renderer.allowRoll = false;
            renderer.alignment = ParticleSystemRenderSpace.Facing;

            var main = particleSystem.main;
            main.loop = false;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = settings.maxParticleNumber; //Change maxParticlesNumber if you are creating very big heatmap
            main.playOnAwake = false;

            if (settings.particleMaterial != null)
            {
                renderer.material = settings.particleMaterial;
            } 
            else
            {
                UnityEngine.Debug.Log("particle material is not defined!");
            }

            return particleSystem;
        }

        UnityEngine.Debug.Log("there is particle system on parent object already!");

        return null;
    }
    

    public static void InitParticleArray()
    {
        //finding size of collider in particles
        size = Vector3Int.FloorToInt((parentCollider.bounds.max - parentCollider.bounds.min) / settings.particleDistance);
      
        if (settings.heightInParticles != 0)
        {
            size.y = settings.heightInParticles;            
        }

        // this array can be optimised and replaced to not have memory bottleneck with VERY big particle systems
        particles = new ParticleSystem.Particle[size.x, size.y, size.z];
        color = new float[size.x, size.y, size.z];
        

        for (int x = 0; x < size.x; x += 1)
        {
            for (int y = 0; y < size.y; y += 1)
            {
                for (int z = 0; z < size.z; z += 1)
                {
                    ParticleSystem.Particle particle = new ParticleSystem.Particle();

                    Vector3 position = ConvertPositionToGlobal(new Vector3Int(x, y, z));
                    particle.position = position;

                    particle.startSize = settings.particleSize;
                    particle.startColor = settings.gradient.Evaluate(0);

                    particle.remainingLifetime = 1000;
                    particle.startLifetime = 1000;

                    particles[x, y, z] = particle;
                }
            }

        }

        UpdateParticleSystem();
    }



    private static Vector3Int ConvertPositionToGrid (Vector3 position)
    {
        Vector3Int convertedPosition = Vector3Int.RoundToInt((position - parentCollider.bounds.min) / settings.particleDistance);

        return convertedPosition;
    }

    private static Vector3 ConvertPositionToGlobal(Vector3Int position)
    {
        Vector3 convertedPosition;
        convertedPosition.x = (position.x * settings.particleDistance) + parentCollider.bounds.min.x;
        convertedPosition.y = (position.y * settings.particleDistance) + parentCollider.bounds.min.y;
        convertedPosition.z = (position.z * settings.particleDistance) + parentCollider.bounds.min.z;

        return convertedPosition;
    }

    public static void ResetParticlesColor()
    {
        color = new float[size.x, size.y, size.z];
    }


    private static ParticleSystem.Particle[] ConvertGridToArray(ParticleSystem.Particle[,,] grid)
    {
        List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();

        for (int x = 0; x < size.x; x += 1)
        {
            for (int y = 0; y < size.y; y += 1)
            {
                for (int z = 0; z < size.z; z += 1)
                {
                    particleList.Add(particles[x, y, z]);
                }
            }
        }

        return particleList.ToArray();
    }

    public static void UpdateParticleSystem()
    {
        List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();

        for (int x = 0; x < size.x; x += 1)
        {
            for (int y = 0; y < size.y; y += 1)
            {
                for (int z = 0; z < size.z; z += 1)
                {   
                    // the invisible particles / particles with color value lower than colorCutoff should NOT be added to particle system
                    if (settings.colorCutoff <= color[x, y, z])
                    {
                        Color particleColor = new Color();
                        particleColor = settings.gradient.Evaluate(color[x, y, z]);
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

    private static void AddOnePositionToHeatmap(EventPosition eventPosition)
    {
        Vector3Int pointInGrid = ConvertPositionToGrid(eventPosition.positionVector);

        Vector3Int min = new Vector3Int();
        Vector3Int max = new Vector3Int();

        // calculating size of the box that we will check particles in
        min.x = (int)(pointInGrid.x - settings.maxColoringDistance / settings.particleDistance - 1);
        min.z = (int)(pointInGrid.z - settings.maxColoringDistance / settings.particleDistance - 1);
        max.x = (int)(pointInGrid.x + settings.maxColoringDistance / settings.particleDistance + 1);
        max.z = (int)(pointInGrid.z + settings.maxColoringDistance / settings.particleDistance + 1);

        if (!settings.ignoreYforColoring)
        {
            max.y = (int)(pointInGrid.y + settings.maxColoringDistance / settings.particleDistance + 1);
            min.y = (int)(pointInGrid.y - settings.maxColoringDistance / settings.particleDistance - 1);
        }
        else
        {
            min.y = 0;
            max.y = size.y - 1;
        }

        // checking all particles in box for distance to point (selection should look like sphere)
        for (int x = min.x; x <= max.x; x += 1)
        {
            for (int y = min.y; y <= max.y; y += 1)
            {
                for (int z = min.z; z <= max.z; z += 1)
                {
                    // check if position inside of particle grid and calculate colorAddValue from distance
                    if (x >= 0 && z >= 0 && y >= 0 && x < size.x && y < size.y & z < size.z)
                    {
                        float distance = ((pointInGrid.x - x) * (pointInGrid.x - x) + (pointInGrid.z - z) * (pointInGrid.z - z));
                        if (!settings.ignoreYforColoring)
                        {
                            distance = distance + (pointInGrid.y - y) * (pointInGrid.y - y);
                        }
                        if (distance <= (settings.maxColoringDistance * settings.maxColoringDistance))
                        {
                            float colorAddValue = settings.colorMultiplier;
                            if (distance > 1)
                            {
                                colorAddValue = colorAddValue / Mathf.Sqrt(distance);
                            }
                            color[x, y, z] = color[x, y, z] + colorAddValue * eventPosition.positionMultiplier;
                        }
                    }
                }
            }
        }
    }
    

    public static void AddEventToHeatMap(EventData eventData)
    {
        foreach (EventPosition eventPosition in eventData.positions)
        {
            AddOnePositionToHeatmap(eventPosition);
        }

    }

}



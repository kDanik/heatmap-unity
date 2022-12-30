using UnityEngine;

public class HeatmapVisualisation
{
    private readonly HeatmapController.Settings settings;

    private HeatmapParticleSystem heatmapParticleSystem;

    private float[,,] particleColorValues;

    public HeatmapVisualisation(HeatmapController.Settings settings)
    {
        this.settings = settings;

        heatmapParticleSystem = new HeatmapParticleSystem();
    }

    /// <summary>
    /// Creates and configures particle system for heatmap visualisation.
    /// </summary>
    /// <param name="parent">Object that will contain ParticleSystem component</param>
    public void InitializeParticleSystem(GameObject parent)
    {
        if (parent.GetComponent<ParticleSystem>() != null)
        {
            Debug.Log("There is particle system present on parent object already!");

            return;
        }

        if (settings.particleMaterial == null)
        {
            Debug.LogError("Particle material is not defined in settings!");

            return;
        }

        heatmapParticleSystem.InitializeParticleSystem(parent, settings);
    }

    /// <summary>
    /// Initializes particle array and populates it with particles with default color value.
    /// </summary>
    public void InitializeParticleArray()
    {
        heatmapParticleSystem.CreateParticleArray(settings);

        Vector3Int sizeInParticles = heatmapParticleSystem.GetSizeOfParticleSystemInParticles();
        particleColorValues = new float[sizeInParticles.x, sizeInParticles.y, sizeInParticles.z];

        UpdateParticlesInParticleSystem();
    }

    /// <summary>
    /// Adds all positions from eventData to heatmap (by calculating new color values for affected particles)
    /// </summary>
    public void AddEventToHeatMap(EventData eventData)
    {
        foreach (MergedEventPosition eventPosition in eventData.Positions)
        {
            AddOnePositionToHeatmap(eventPosition);
        }
    }


    /// <summary>
    /// Resets color value of all particles to default (0f)
    /// </summary>
    public void ResetParticlesColor()
    {
        Vector3Int sizeInParticles = heatmapParticleSystem.GetSizeOfParticleSystemInParticles();
        particleColorValues = new float[sizeInParticles.x, sizeInParticles.y, sizeInParticles.z];
    }

    public void UpdateParticlesInParticleSystem()
    {
        heatmapParticleSystem.UpdateParticlesInParticleSystem(particleColorValues, settings);
    }

    private void AddOnePositionToHeatmap(MergedEventPosition eventPosition)
    {
        Vector3Int pointInGrid = heatmapParticleSystem.ConvertGlobalPositionToParticleGrid(eventPosition.Position, settings);
        Vector3Int sizeInParticles = heatmapParticleSystem.GetSizeOfParticleSystemInParticles();

        // calculate bounds in which particles can be affected by eventPosition
        Vector3Int minBound = CalculateMinBound(pointInGrid);
        Vector3Int maxBound = CalculateMaxBound(pointInGrid);

        // checking all particles in this bounds, and updating their color value depending on distance
        for (int x = minBound.x; x <= maxBound.x; x += 1)
        {
            for (int y = minBound.y; y <= maxBound.y; y += 1)
            {
                for (int z = minBound.z; z <= maxBound.z; z += 1)
                {
                    if (IsInBoundsOfParticleArray(x, y, z, sizeInParticles))
                    {
                        UpdateColorAddValue(x, y, z, pointInGrid, eventPosition);
                    }
                }
            }
        }
    }

    private void UpdateColorAddValue(int xGrid, int yGrid, int zGrid, Vector3Int pointInGrid, MergedEventPosition eventPosition)
    {
        float distance = ((pointInGrid.x - xGrid) * (pointInGrid.x - xGrid) + (pointInGrid.z - zGrid) * (pointInGrid.z - zGrid));

        if (!settings.ignoreYforColoring)
        {
            distance += (pointInGrid.y - yGrid) * (pointInGrid.y - yGrid);
        }
            
        if (distance <= (settings.maxColoringDistance * settings.maxColoringDistance))
        {
            float colorAddValue = settings.colorMultiplier;
            if (distance > 1)
            {
                colorAddValue /= Mathf.Sqrt(distance);
            }
            particleColorValues[xGrid, yGrid, zGrid] = particleColorValues[xGrid, yGrid, zGrid] + colorAddValue * eventPosition.Multiplier;
        }
    }

    private bool IsInBoundsOfParticleArray(int x, int y, int z, Vector3Int sizeInParticles)
    {
        return x >= 0 && z >= 0 && y >= 0 && x < sizeInParticles.x && y < sizeInParticles.y & z < sizeInParticles.z;
    }

    private Vector3Int CalculateMinBound(Vector3Int positionInParticleGrid)
    {
        Vector3Int min = new();

        min.x = (int)(positionInParticleGrid.x - settings.maxColoringDistance / settings.particleDistance - 1);
        min.z = (int)(positionInParticleGrid.z - settings.maxColoringDistance / settings.particleDistance - 1);

        if (!settings.ignoreYforColoring)
        {
            min.y = (int)(positionInParticleGrid.y - settings.maxColoringDistance / settings.particleDistance - 1);
        }
        else
        {
            min.y = 0;
        }

        return min;
    }

    private Vector3Int CalculateMaxBound(Vector3Int positionInParticleGrid)
    {
        Vector3Int max = new();

        max.x = (int)(positionInParticleGrid.x + settings.maxColoringDistance / settings.particleDistance + 1);
        max.z = (int)(positionInParticleGrid.z + settings.maxColoringDistance / settings.particleDistance + 1);

        if (!settings.ignoreYforColoring)
        {
            max.y = (int)(positionInParticleGrid.y + settings.maxColoringDistance / settings.particleDistance + 1);
        }
        else
        {
            max.y = positionInParticleGrid.y - 1;
        }

        return max;
    }
}
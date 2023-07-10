using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HeatmapController : MonoBehaviour
{
    [Serializable]
    public class Settings
    {
        public int heightInParticles;
        public float maxColoringDistance;
        public bool ignoreYforColoring = false;
        public Gradient gradient;
        public float particleDistance;
        public float colorMultiplier;
        public float particleSize;
        public float colorCutoff;
        public Material particleMaterial;
        public int maxParticleNumber = 50000;

        public string pathForReadingData;
    }
    public Settings settings = new();

    public List<EventData> events = new();

    private IEventReader eventReader;
    private HeatmapVisualisation heatmapVisualisation;

    private bool eventsAreLoaded = false;
    private bool particleSystemIsInitialized = false;

    private void Awake()
    {
        heatmapVisualisation = new HeatmapVisualisation(settings);
    }

    /// <summary>
    /// Loads events from file into events property (that also makes them display in heatmap configuration)
    /// </summary>
    public void LoadEvents()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        eventReader = new JSONEventReader(settings.pathForReadingData);

        if (eventReader.ReaderIsAvailable())
        {
            events = eventReader.ReadEvents();
            eventsAreLoaded = true;
        }
        else
        {
            eventsAreLoaded = false;
            Debug.Log("Error while trying to read events. Event reader is not available");
        }

        stopwatch.Stop();
        Debug.Log("LoadEvents - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Creates and configures particle system (and particle array)
    /// </summary>
    public void InitializeParticleSystem()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        heatmapVisualisation.InitializeParticleSystem(gameObject);
        heatmapVisualisation.InitializeParticleArray();
        particleSystemIsInitialized = true;

        stopwatch.Stop();
        Debug.Log("InitializeParticleSystem - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Resets heatmap color(color values) to default
    /// </summary>
    public void ResetHeatmap()
    {
        heatmapVisualisation.ResetParticlesColor();
        heatmapVisualisation.UpdateParticlesInParticleSystem();
    }

    /// <summary>
    /// Adds selected (in Editor window) events to heatmap and updates heatmap with their values
    /// </summary>
    public void AddSelectedEventsToHeatmap()
    {

        Stopwatch stopwatch = new();
        stopwatch.Start();

        heatmapVisualisation.ResetParticlesColor();

        foreach (EventData eventData in events)
        {
            if (eventData.ShouldEventBeVisualised)
            {
                heatmapVisualisation.AddEventToHeatMap(eventData);
            }
        }

        heatmapVisualisation.UpdateParticlesInParticleSystem();

        stopwatch.Stop();
        Debug.Log("AddEventsToHeatMap  - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }

    /// <summary>
    /// Status of Load Events action. (see HeatmapGUI.cs for usage))
    /// </summary>
    public bool IsLoadEventsActive()
    {
        return !string.IsNullOrEmpty(settings.pathForReadingData);
    }

    /// <summary>
    /// Status of Initialize Particle System action.  (see HeatmapGUI.cs for usage)
    /// </summary>
    public bool IsInitializeParticleSystemActive()
    {
        return GetComponent<BoxCollider>() != null;
    }

    /// <summary>
    /// Status of Add Events to Heatmap action.  (see HeatmapGUI.cs for usage)
    /// </summary>
    public bool IsAddEventToHeatMapActive()
    {
        return eventsAreLoaded && particleSystemIsInitialized;
    }

    /// <summary>
    /// Status of Reset Heatmap action. (see HeatmapGUI.cs for usage)
    /// </summary>
    public bool IsResetHeatmapActive()
    {
        return particleSystemIsInitialized;
    }
}
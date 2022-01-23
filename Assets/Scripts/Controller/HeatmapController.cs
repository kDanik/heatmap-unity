using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;


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
    public Settings settings = new Settings();

    public List<EventData> events = new List<EventData>();


    private bool eventsAreLoaded = false;
    private bool particleSystemInitialized = false;

    public void LoadEvents()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        events = HeatMapFileUtil.ReadVectorsFromFile(settings.pathForReadingData);
        eventsAreLoaded = true;

        stopwatch.Stop();
        UnityEngine.Debug.Log("LoadEvents - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }
    public void InitializeParticleSystem()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        HeatmapVisualisation.InitParticleSystem(this.gameObject);
        HeatmapVisualisation.InitParticleArray();
        particleSystemInitialized = true;

        stopwatch.Stop();
        UnityEngine.Debug.Log("InitializeParticleSystem - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }

    public void UpdateParticleSystem()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        HeatmapVisualisation.UpdateParticleSystem();

        stopwatch.Stop();
        UnityEngine.Debug.Log("UpdateParticleSystem - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }


    public void ResetHeatmap()
    {
        HeatmapVisualisation.ResetParticlesColor();
        HeatmapVisualisation.UpdateParticleSystem();
    }

    public void AddSelectedEventsToHeatmap()
    {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        HeatmapVisualisation.ResetParticlesColor();

        foreach (EventData eventData in events)
        {
            if (eventData.active)
            {
                HeatmapVisualisation.AddEventToHeatMap(eventData);
            }
        }

        HeatmapVisualisation.UpdateParticleSystem();

        stopwatch.Stop();
        UnityEngine.Debug.Log("AddEventsToHeatMap  - Elapsed Time is " + stopwatch.ElapsedMilliseconds + " ms");
    }

    public bool IsLoadEventsActive()
    {
        return !String.IsNullOrEmpty(settings.pathForReadingData);
    }

    public bool IsInitializeParticleSystemActive()
    {
        return GetComponent<BoxCollider>() != null;

    }

    public bool IsAddEventToHeatMapActive()
    {
        return eventsAreLoaded && particleSystemInitialized;
    }

    public bool IsUpdateParticleSystemActive()
    {
        return particleSystemInitialized;
    }

    public bool IsResetHeatmapActive()
    {
        return particleSystemInitialized;
    }

}


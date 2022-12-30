using UnityEditor;
using UnityEngine;

/// <summary>
/// Controls Heatmap controller GUI in Editor window
/// </summary>
[CustomEditor(typeof(HeatmapController))]
public class HeatmapGUI : Editor
{
    private HeatmapController heatmapController;

    private SerializedObject serializedGradient;

    public override void OnInspectorGUI()
    {
        heatmapController = (HeatmapController)target;

        AddMethodButtons();

        // set enabled to true to prevent buttons enabled settings to affect other elements below them
        GUI.enabled = true;

        AddEventsSelect();

        AddNormalSettings();

        AddAdvancedSettings();

        // gradient should be update in the end of OnInspectorGUI
        serializedGradient.ApplyModifiedProperties();
    }

    private void AddMethodButtons()
    {
        GUILayout.Label("\nMethods", EditorStyles.boldLabel);

        AddLoadEventsButton();
        AddInitializeParticleSystemButton();
        AddGenerateHeatmapButton();
        AddUpdateHeatmapButton();
        AddResetHeatmapButton();
    }

    private void AddLoadEventsButton()
    {
        GUI.enabled = heatmapController.IsLoadEventsActive();
        if (GUILayout.Button(new GUIContent("Load events from file", "")))
        {
            if (Application.isPlaying)
            {
                heatmapController.LoadEvents();
            }
        }
    }

    private void AddResetHeatmapButton()
    {
        GUI.enabled = heatmapController.IsResetHeatmapActive();
        if (GUILayout.Button(new GUIContent("Reset heatmap", "Resets heatmap to default values")))
        {
            if (Application.isPlaying)
            {
                heatmapController.ResetHeatmap();
            }
        }
    }

    private void AddInitializeParticleSystemButton()
    {
        GUI.enabled = heatmapController.IsInitializeParticleSystemActive();
        if (GUILayout.Button(new GUIContent("Initialize particle system", "Initializes particle system and prepares particle array")))
        {
            if (Application.isPlaying)
            {
                heatmapController.InitializeParticleSystem();
            }
        }
    }

    private void AddGenerateHeatmapButton()
    {
        GUI.enabled = heatmapController.IsAddEventToHeatMapActive();
        if (GUILayout.Button(new GUIContent("Generate heatmap", "Calculates color of particles in particle system")))
        {
            if (Application.isPlaying)
            {
                heatmapController.AddSelectedEventsToHeatmap();
            }
        }
    }

    private void AddUpdateHeatmapButton()
    {

        GUI.enabled = heatmapController.IsUpdateParticleSystemActive();
        if (GUILayout.Button(new GUIContent("Update heatmap", "Applies color calculations to heatmap")))
        {
            if (Application.isPlaying)
            {
                heatmapController.UpdateParticleSystem();
            }
        }

    }

    private void AddEventsSelect()
    {
        GUILayout.Label("\nEvents", EditorStyles.boldLabel);
        GUILayout.Label("Choose events that you want to display with heatmap\n");
        if (heatmapController.events != null)
        {
            foreach (EventData eventData in heatmapController.events)
            {
                eventData.IsCurrentlyDisplayedOnHeatmap = EditorGUILayout.Toggle(eventData.EventName, eventData.IsCurrentlyDisplayedOnHeatmap);
            }
        }
    }

    private void AddNormalSettings()
    {
        GUILayout.Label("\nSettings", EditorStyles.boldLabel);

        heatmapController.settings.particleDistance = EditorGUILayout.Slider(new GUIContent("Distance between particles", "Distance for any of axes from one particle to other one. Make it less if you want to improve performance"), heatmapController.settings.particleDistance, 0.1F, 5F);

        heatmapController.settings.particleSize = EditorGUILayout.Slider(new GUIContent("Particle Size", "(Size of particle in units)"), heatmapController.settings.particleSize, 0.01F, 15F);

        heatmapController.settings.colorMultiplier = EditorGUILayout.Slider(new GUIContent("Coloring Multiplier", "Defines how much one point will affect colors of particles in heatmap "), heatmapController.settings.colorMultiplier, 0, 1F);

        heatmapController.settings.maxColoringDistance = EditorGUILayout.Slider(new GUIContent("Coloring Distance", "(Max distance between particle and vector3 that effects it)"), heatmapController.settings.maxColoringDistance, 0.01F, 10F);

        serializedGradient = new SerializedObject(target);
        SerializedProperty colorGradient = serializedGradient.FindProperty("settings.gradient");
        EditorGUILayout.PropertyField(colorGradient, true, null);
    }

    private void AddAdvancedSettings()
    {
        GUILayout.Label("\nAdvanced Settings", EditorStyles.boldLabel);

        heatmapController.settings.colorCutoff = EditorGUILayout.Slider(new GUIContent("Color Cutoff", "Minimum color value that will be displayed(With 0 value cutout is deactivated)"), heatmapController.settings.colorCutoff, 0F, 1.01F);

        heatmapController.settings.heightInParticles = EditorGUILayout.IntSlider(new GUIContent("Height in particles", "(With 0 value height is calculated depending on collider height)"), heatmapController.settings.heightInParticles, 0, 50);

        heatmapController.settings.ignoreYforColoring = EditorGUILayout.Toggle(new GUIContent("Ignore height for color calculations", "(If true color will be calculated only depending on X and Z axes)"), heatmapController.settings.ignoreYforColoring);

        GUILayout.Label("\nPaths", EditorStyles.boldLabel);

        GUILayout.Label("Path for reading points from file");
        heatmapController.settings.pathForReadingData = GUILayout.TextField(heatmapController.settings.pathForReadingData);


        GUILayout.Label("\nMaterial", EditorStyles.boldLabel);

        GUILayout.Label("Use only the one from package(or be creative if you know what you are doing)");

        heatmapController.settings.particleMaterial = (Material)EditorGUILayout.ObjectField(heatmapController.settings.particleMaterial, typeof(Material), true);
    }

}

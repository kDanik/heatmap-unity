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

        // set enabled to true to prevent buttons 'enabled' setting to affect other elements below them
        GUI.enabled = true;

        AddEventsSelect();

        AddNormalSettings();

        AddAdvancedSettings();

        // gradient changes must be applied in the end of OnInspectorGUI
        serializedGradient.ApplyModifiedProperties();
    }

    private void AddMethodButtons()
    {
        GUILayout.Label("\nActions", EditorStyles.boldLabel);

        AddLoadEventsButton();
        AddInitializeParticleSystemButton();
        AddGenerateHeatmapButton();
        AddResetHeatmapButton();
    }

    private void AddLoadEventsButton()
    {
        GUI.enabled = heatmapController.IsLoadEventsActive();
        if (GUILayout.Button(new GUIContent("Load events from file")))
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
        if (GUILayout.Button(new GUIContent("Reset heatmap values", "Resets heatmap color values to default values")))
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
        if (GUILayout.Button(new GUIContent("Generate heatmap", "Calculates color of particles in particle system using data from selected events")))
        {
            if (Application.isPlaying)
            {
                heatmapController.AddSelectedEventsToHeatmap();
            }
        }
    }

    private void AddEventsSelect()
    {
        GUILayout.Label("\nEvents", EditorStyles.boldLabel);
        GUILayout.Label("Choose events, that should be visualized with heatmap\n");

        if (heatmapController.events != null)
        {
            foreach (EventData eventData in heatmapController.events)
            {
                eventData.ShouldEventBeVisualised = EditorGUILayout.Toggle(eventData.EventName, eventData.ShouldEventBeVisualised);
            }
        }
    }

    private void AddNormalSettings()
    {
        GUILayout.Label("\nSettings", EditorStyles.boldLabel);

        heatmapController.settings.particleDistance = EditorGUILayout.Slider(new GUIContent("Distance between particles", "Smaller distance - improved visuals and precision. Bigger distance - improved performance"), heatmapController.settings.particleDistance, 0.1F, 5F);

        heatmapController.settings.particleSize = EditorGUILayout.Slider(new GUIContent("Particle Size", "(in Unity units)"), heatmapController.settings.particleSize, 0.01F, 15F);

        heatmapController.settings.colorMultiplier = EditorGUILayout.Slider(new GUIContent("Coloring Multiplier", "Defines how much one position will change color value of particles near it"), heatmapController.settings.colorMultiplier, 0, 1F);

        heatmapController.settings.maxColoringDistance = EditorGUILayout.Slider(new GUIContent("Coloring Distance", "Max distance in which event position will affect color of particles"), heatmapController.settings.maxColoringDistance, 0.01F, 15F);

        serializedGradient = new SerializedObject(target);
        SerializedProperty colorGradient = serializedGradient.FindProperty("settings.gradient");
        EditorGUILayout.PropertyField(colorGradient, true, null);
    }

    private void AddAdvancedSettings()
    {
        GUILayout.Label("\nAdvanced Settings", EditorStyles.boldLabel);

        heatmapController.settings.colorCutoff = EditorGUILayout.Slider(new GUIContent("Color Cutoff", "Hides all particles with smaller color value (With 0 value cutout is deactivated)"), heatmapController.settings.colorCutoff, 0F, 1.01F);

        heatmapController.settings.heightInParticles = EditorGUILayout.IntSlider(new GUIContent("Height of particle system in particles", "(With 0 value height is calculated depending on collider height)"), heatmapController.settings.heightInParticles, 0, 50);

        heatmapController.settings.ignoreYforColoring = EditorGUILayout.Toggle(new GUIContent("Ignore height for color calculations", "(If true color will be calculated only depending on X and Z axes)"), heatmapController.settings.ignoreYforColoring);

        GUILayout.Label("\nPaths", EditorStyles.boldLabel);

        GUILayout.Label("Path for reading event data from file");
        heatmapController.settings.pathForReadingData = GUILayout.TextField(heatmapController.settings.pathForReadingData);

        GUILayout.Label("\nMaterial", EditorStyles.boldLabel);

        GUILayout.Label("Use material, that is used in example prefab for heatmap. \n(or be creative if you know what you are doing)");

        heatmapController.settings.particleMaterial = (Material)EditorGUILayout.ObjectField(heatmapController.settings.particleMaterial, typeof(Material), true);
    }

}

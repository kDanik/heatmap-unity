
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(HeatmapController))]
public class HeatmapGUI : Editor
{
    public override void OnInspectorGUI()
    {
        HeatmapController controller = (HeatmapController)target;
        //EditorGUI.BeginChangeCheck();

        //everything related to executing actions on the heatmap
        GUILayout.Label("\nMethods", EditorStyles.boldLabel);

        GUI.enabled = controller.IsLoadEventsActive();
        if (GUILayout.Button(new GUIContent("Load events from file", "")))
        {
            if (Application.isPlaying)
            {
                controller.LoadEvents();
            }
        }


        GUI.enabled = controller.IsInitializeParticleSystemActive();
        if (GUILayout.Button(new GUIContent("Initialize particle system", "Initializes particle system and prepares particle array")))
        {
            if (Application.isPlaying)
            {
                controller.InitializeParticleSystem();
            }
        }

        GUI.enabled = controller.IsAddEventToHeatMapActive();
        if (GUILayout.Button(new GUIContent("Generate heatmap", "Calculates color of particles in particle system")))
        {
            if (Application.isPlaying)
            {
                controller.AddSelectedEventsToHeatmap();
            }
        }

        GUI.enabled = controller.IsUpdateParticleSystemActive();
        if (GUILayout.Button(new GUIContent("Update heatmap", "Applies color calculations to heatmap")))
        {
            if (Application.isPlaying)
            {
                controller.UpdateParticleSystem();
            }
        }

        GUI.enabled = controller.IsResetHeatmapActive();
        if (GUILayout.Button(new GUIContent("Reset heatmap", "Resets heatmap to default values")))
        {
            if (Application.isPlaying)
            {
                controller.ResetHeatmap();
            }
        }
        GUI.enabled = true;

        // events that should be counted for color calculation

        GUILayout.Label("\nEvents", EditorStyles.boldLabel);
        GUILayout.Label("Choose events that you want to display with heatmap\n");
        if (controller.events != null)
        {
            foreach (EventData eventData in controller.events)
            {
                eventData.active = EditorGUILayout.Toggle(eventData.name, eventData.active);
            }
        }


        // everything related to settings for generation of the heatmap
        GUILayout.Label("\nSettings", EditorStyles.boldLabel);

        controller.settings.particleDistance = EditorGUILayout.Slider(new GUIContent("Distance between particles", "Distance for any of axes from one particle to other one. Make it less if you want to improve performance"), controller.settings.particleDistance, 0.1F, 5F);

        controller.settings.particleSize = EditorGUILayout.Slider(new GUIContent("Particle Size", "(Size of particle in units)"), controller.settings.particleSize, 0.01F, 15F);

        controller.settings.colorMultiplier = EditorGUILayout.Slider(new GUIContent("Coloring Multiplier", "Defines how much one point will affect colors of particles in heatmap "), controller.settings.colorMultiplier, 0, 1F);

        controller.settings.maxColoringDistance = EditorGUILayout.Slider(new GUIContent("Coloring Distance", "(Max distance between particle and vector3 that effects it)"), controller.settings.maxColoringDistance, 0.01F, 10F);

        SerializedObject serializedGradient = new SerializedObject(target);
        SerializedProperty colorGradient = serializedGradient.FindProperty("settings.gradient");
        EditorGUILayout.PropertyField(colorGradient, true, null);


        GUILayout.Label("\nAdvanced Settings", EditorStyles.boldLabel);

        controller.settings.colorCutoff = EditorGUILayout.Slider(new GUIContent("Color Cutoff", "Minimum color value that will be displayed(With 0 value cutout is deactivated)"), controller.settings.colorCutoff, 0F, 1.01F);

        controller.settings.heightInParticles = EditorGUILayout.IntSlider(new GUIContent("Height in particles", "(With 0 value height is calculated depending on collider height)"), controller.settings.heightInParticles, 0, 50);

        controller.settings.ignoreYforColoring = EditorGUILayout.Toggle(new GUIContent("Ignore height for color calculations", "(If true color will be calculated only depending on X and Z axes)"), controller.settings.ignoreYforColoring);

        GUILayout.Label("\nPaths", EditorStyles.boldLabel);

        GUILayout.Label("Path for reading points from file");
        controller.settings.pathForReadingData = GUILayout.TextField(controller.settings.pathForReadingData);


        GUILayout.Label("\nMaterial", EditorStyles.boldLabel);

        GUILayout.Label("Use only the one from package(or be creative if you know what you are doing)");

        controller.settings.particleMaterial = (Material)EditorGUILayout.ObjectField(controller.settings.particleMaterial, typeof(Material), true);


        serializedGradient.ApplyModifiedProperties();
    }
}

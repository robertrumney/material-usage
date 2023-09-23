using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

// An Editor Window to display and manage the material usage in the scene.
public class MaterialUsageWindow : EditorWindow
{
    // For managing the scroll position of the window.
    private Vector2 scrollPos;

    // Dictionary to keep track of each material and the MeshRenderers that use it.
    private Dictionary<Material, List<MeshRenderer>> materialUsage = new Dictionary<Material, List<MeshRenderer>>();

    // Variables to display statistics on the UI.
    private int totalMaterialsUsed;
    private int totalMeshRenderers;
    private int totalMaterialsCounted;

    // Threshold to filter materials based on their usage count.
    private int usageThreshold = 20;

    // Whether to limit the material count based on the usage threshold.
    private bool limitCount = false;

    // Whether to ping/highlight the MeshRenderers in the scene.
    private bool pingMeshes = false;

    // Creates an editor window accessible via Window > Material Usage
    [MenuItem("Window/Material Usage")]
    public static void ShowWindow()
    {
        GetWindow<MaterialUsageWindow>("Material Usage");
    }

    // GUI for the editor window.
    private void OnGUI()
    {
        // Button to trigger the scanning of material usage.
        if (GUILayout.Button("Get Material Usage"))
        {
            GetMaterialUsage();
        }

        // Display statistics.
        EditorGUILayout.LabelField("Total Unique Materials Used: " + totalMaterialsUsed);
        EditorGUILayout.LabelField("Total MeshRenderers: " + totalMeshRenderers);
        EditorGUILayout.LabelField("Total Materials Counted: " + totalMaterialsCounted);

        // Controls for filtering and options.
        limitCount = EditorGUILayout.Toggle("Limit Count", limitCount);
        usageThreshold = EditorGUILayout.IntField("Usage Threshold", usageThreshold);
        pingMeshes = EditorGUILayout.Toggle("Ping Meshes", pingMeshes);

        // Start of scroll view for displaying materials and their usage.
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        // Order the materials based on their usage count.
        var sortedMaterialUsage = materialUsage.OrderBy(x => x.Value.Count).ToList();

        foreach (KeyValuePair<Material, List<MeshRenderer>> entry in sortedMaterialUsage)
        {
            int count = entry.Value.Count;

            // Skip entries that exceed the usage threshold if the limit is set.
            if (limitCount && count > usageThreshold)
                continue;

            // Display the material and its usage count.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(entry.Key, typeof(Material), false);
            EditorGUILayout.LabelField(" | Usage Count: " + count);

            // If 'Ping Meshes' is checked, show the first MeshRenderer that uses the material.
            if (pingMeshes && entry.Value.Count > 0)
            {
                EditorGUILayout.ObjectField(entry.Value[0], typeof(MeshRenderer), false);
            }
            EditorGUILayout.EndHorizontal();
        }

        // End of scroll view.
        EditorGUILayout.EndScrollView();
    }

    // Method to gather and compute the material usage in the scene.
    private void GetMaterialUsage()
    {
        // Clear previous data.
        materialUsage.Clear();
        totalMeshRenderers = 0;
        totalMaterialsCounted = 0;

        // Iterate over all MeshRenderers in the scene.
        foreach (MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            totalMeshRenderers++;

            // For each material in the renderer, update its usage.
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material != null)
                {
                    if (!materialUsage.ContainsKey(material))
                    {
                        materialUsage[material] = new List<MeshRenderer> { renderer };
                    }
                    else
                    {
                        materialUsage[material].Add(renderer);
                    }
                }
            }
        }

        // Update the total materials used.
        totalMaterialsUsed = materialUsage.Count;

        // Compute the total materials counted based on the set threshold.
        if (limitCount)
        {
            totalMaterialsCounted = materialUsage.Count(pair => pair.Value.Count <= usageThreshold);
        }
        else
        {
            totalMaterialsCounted = totalMaterialsUsed;
        }
    }
}

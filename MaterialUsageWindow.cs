using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

public class MaterialUsageWindow : EditorWindow
{
    private Vector2 scrollPos;
    private Dictionary<Material, List<MeshRenderer>> materialUsage = new Dictionary<Material, List<MeshRenderer>>();

    private int totalMaterialsUsed;
    private int totalMeshRenderers;
    private int totalMaterialsCounted;
    private int usageThreshold = 20;

    private bool limitCount = false;
    private bool pingMeshes = false;

    [MenuItem("Window/Material Usage")]
    public static void ShowWindow()
    {
        GetWindow<MaterialUsageWindow>("Material Usage");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Get Material Usage"))
        {
            GetMaterialUsage();
        }

        EditorGUILayout.LabelField("Total Unique Materials Used: " + totalMaterialsUsed);
        EditorGUILayout.LabelField("Total MeshRenderers: " + totalMeshRenderers);
        EditorGUILayout.LabelField("Total Materials Counted: " + totalMaterialsCounted);

        limitCount = EditorGUILayout.Toggle("Limit Count", limitCount);
        usageThreshold = EditorGUILayout.IntField("Usage Threshold", usageThreshold);
        pingMeshes = EditorGUILayout.Toggle("Ping Meshes", pingMeshes);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        var sortedMaterialUsage = materialUsage.OrderBy(x => x.Value.Count).ToList();

        foreach (KeyValuePair<Material, List<MeshRenderer>> entry in sortedMaterialUsage)
        {
            int count = entry.Value.Count;

            if (limitCount && count > usageThreshold)
                continue;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField(entry.Key, typeof(Material), false);
            EditorGUILayout.LabelField(" | Usage Count: " + count);
            if (pingMeshes && entry.Value.Count > 0)
            {
                EditorGUILayout.ObjectField(entry.Value[0], typeof(MeshRenderer), false);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void GetMaterialUsage()
    {
        materialUsage.Clear();
        totalMeshRenderers = 0;
        totalMaterialsCounted = 0;

        foreach (MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            totalMeshRenderers++;
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

        totalMaterialsUsed = materialUsage.Count;

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

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SelectObjectsByMaterial : EditorWindow
{
    private Material selectedMaterial;

    [MenuItem("Tools/Select Objects by Material")]
    public static void ShowWindow()
    {
        GetWindow<SelectObjectsByMaterial>("Select Objects by Material");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Material", EditorStyles.boldLabel);
        selectedMaterial = (Material)EditorGUILayout.ObjectField("Material", selectedMaterial, typeof(Material), false);

        if (GUILayout.Button("Select Objects"))
        {
            SelectObjectsUsingMaterial();
        }
    }

    private void SelectObjectsUsingMaterial()
    {
        if (selectedMaterial == null)
        {
            Debug.LogWarning("Please select a material first.");
            return;
        }

        List<GameObject> objectsWithMaterial = new List<GameObject>();
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                if (mat == selectedMaterial)
                {
                    objectsWithMaterial.Add(renderer.gameObject);
                    break;
                }
            }
        }

        if (objectsWithMaterial.Count > 0)
        {
            Selection.objects = objectsWithMaterial.ToArray();
            Debug.Log($"Selected {objectsWithMaterial.Count} object(s) with the material '{selectedMaterial.name}'.");
        }
        else
        {
            Debug.Log("No objects found with the selected material.");
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MaterialReplacer : EditorWindow
{
    [MenuItem("Tools/Replace Duplicated Materials")]
    public static void ReplaceMaterials()
    {
        // Specify the path to the folder containing your original materials
        string materialsFolderPath = "Assets/Models/Material";
        var materials = AssetDatabase.FindAssets("t:Material", new[] { materialsFolderPath });

        // Dictionary to hold original materials by name
        var originalMaterials = new Dictionary<string, Material>();

        // Load original materials into the dictionary
        foreach (var materialGUID in materials)
        {
            var materialPath = AssetDatabase.GUIDToAssetPath(materialGUID);
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            // Add the material to the dictionary if it's not already present
            if (!originalMaterials.ContainsKey(material.name))
            {
                originalMaterials[material.name] = material;
            }
        }

        // Check all renderers in the scene
        var renderers = FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            var sharedMaterials = renderer.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i] != null)
                {
                    // Check if the material has a duplicate suffix pattern
                    string duplicateName = GetDuplicateName(sharedMaterials[i].name);

                    // If an original material with this base name exists, replace the duplicate
                    if (duplicateName != null && originalMaterials.ContainsKey(duplicateName))
                    {
                        Debug.Log($"Replacing material {sharedMaterials[i].name} in object {renderer.gameObject.name} with {duplicateName}");
                        sharedMaterials[i] = originalMaterials[duplicateName];
                    }
                }
            }
            renderer.sharedMaterials = sharedMaterials;
        }

        Debug.Log("Material replacement complete.");
    }

    private static string GetDuplicateName(string materialName)
    {
        // Check for a pattern like "Material1 1" and return "Material1" as the base name
        int index = materialName.LastIndexOf(' ');
        if (index != -1)
        {
            string baseName = materialName.Substring(0, index);
            string suffix = materialName.Substring(index + 1);

            // Check if the suffix is a valid number (i.e., indicating a duplicate)
            if (int.TryParse(suffix, out _))
            {
                return baseName; // Return the base name if it's a duplicate pattern
            }
        }
        return null; // Not a duplicate pattern
    }
}

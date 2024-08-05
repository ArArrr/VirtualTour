using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FBXMaterialHandler : EditorWindow
{
    [MenuItem("Tools/Replace Duplicated Materials in FBX")]
    public static void ReplaceFBXMaterials()
    {
        // Specify the path to the folder containing your FBX assets
        string fbxFolderPath = "Assets/Models"; // Adjust this to your actual FBX folder path
        var fbxAssets = AssetDatabase.FindAssets("t:Model", new[] { fbxFolderPath });

        // Specify the path to the folder containing your original materials
        string materialsFolderPath = "Assets/Models/Material";
        var materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { materialsFolderPath });

        // Dictionary to hold original materials by name
        var originalMaterials = new Dictionary<string, Material>();

        // Load original materials into the dictionary
        foreach (var materialGUID in materialGUIDs)
        {
            var materialPath = AssetDatabase.GUIDToAssetPath(materialGUID);
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            // Add the material to the dictionary if it's not already present
            if (!originalMaterials.ContainsKey(material.name))
            {
                originalMaterials[material.name] = material;
            }
        }

        // Process each FBX asset
        foreach (var fbxGUID in fbxAssets)
        {
            var fbxPath = AssetDatabase.GUIDToAssetPath(fbxGUID);
            var importer = AssetImporter.GetAtPath(fbxPath) as ModelImporter;

            if (importer != null)
            {
                var materialMap = importer.GetExternalObjectMap();

                foreach (var entry in materialMap)
                {
                    if (entry.Value is Material material)
                    {
                        string duplicateName = GetDuplicateName(material.name);

                        if (duplicateName != null && originalMaterials.ContainsKey(duplicateName))
                        {
                            Debug.Log($"Replacing material {material.name} in FBX {fbxPath} with {duplicateName}");
                            importer.RemoveRemap(entry.Key);
                            importer.AddRemap(entry.Key, originalMaterials[duplicateName]);
                        }
                    }
                }

                // Reimport the FBX asset to apply changes
                AssetDatabase.ImportAsset(fbxPath, ImportAssetOptions.ForceUpdate);
            }
        }

        Debug.Log("FBX material replacement complete.");
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

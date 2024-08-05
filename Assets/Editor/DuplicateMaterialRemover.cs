using UnityEngine;
using UnityEditor;

public class DuplicateMaterialRemover : EditorWindow
{
    [MenuItem("Tools/Remove Duplicate Materials")]
    public static void RemoveDuplicateMaterials()
    {
        string materialsFolderPath = "Assets/Models/Material"; // Adjust to your actual materials folder path
        var materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { materialsFolderPath });

        foreach (var materialGUID in materialGUIDs)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialGUID);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            if (material != null)
            {
                string baseMaterialName = GetBaseMaterialName(material.name);
                if (IsDuplicateMaterial(material.name, baseMaterialName))
                {
                    Material baseMaterial = FindBaseMaterial(materialsFolderPath, baseMaterialName);
                    if (baseMaterial != null)
                    {
                        Debug.Log($"Removing duplicate material: {material.name}");
                        AssetDatabase.DeleteAsset(materialPath);
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Duplicate material removal complete.");
    }

    private static string GetBaseMaterialName(string materialName)
    {
        int lastSpaceIndex = materialName.LastIndexOf(' ');
        if (lastSpaceIndex > 0)
        {
            string possibleNumber = materialName.Substring(lastSpaceIndex + 1);
            if (int.TryParse(possibleNumber, out _))
            {
                return materialName.Substring(0, lastSpaceIndex);
            }
        }
        return materialName;
    }

    private static bool IsDuplicateMaterial(string materialName, string baseMaterialName)
    {
        return materialName != baseMaterialName;
    }

    private static Material FindBaseMaterial(string folderPath, string baseMaterialName)
    {
        var materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { folderPath });
        foreach (var materialGUID in materialGUIDs)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialGUID);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (material != null && material.name == baseMaterialName)
            {
                return material;
            }
        }
        return null;
    }
}

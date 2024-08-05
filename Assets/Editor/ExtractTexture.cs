using UnityEngine;
using UnityEditor;
using System.IO;

public class ExtractTextures : Editor
{
    [MenuItem("Tools/Extract and Consolidate Textures")]
    public static void ExtractTexturesFromFBX()
    {
        // Specify the folder where textures will be extracted to
        string targetFolder = "Assets/Models/Textures";

        // Check if the 'Models' folder exists, if not, create it
        if (!AssetDatabase.IsValidFolder("Assets/Models"))
        {
            AssetDatabase.CreateFolder("Assets", "Models");
        }

        // Check if the 'Textures' folder exists under 'Models', if not, create it
        if (!AssetDatabase.IsValidFolder(targetFolder))
        {
            AssetDatabase.CreateFolder("Assets/Models", "Textures");
        }

        // Get all FBX files in the 'Assets/Models' folder
        string[] fbxFiles = Directory.GetFiles("Assets/Models", "*.fbx", SearchOption.AllDirectories);

        foreach (string fbxFile in fbxFiles)
        {
            // Load the FBX file
            GameObject fbxObject = AssetDatabase.LoadAssetAtPath<GameObject>(fbxFile);
            if (fbxObject == null) continue;

            // Get all materials in the FBX object
            Renderer[] renderers = fbxObject.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer renderer in renderers)
            {
                foreach (Material material in renderer.sharedMaterials)
                {
                    if (material == null) continue;

                    // Extract textures from the material
                    ExtractTexturesFromMaterial(material, targetFolder);
                }
            }
        }

        // Refresh the Asset Database to show changes
        AssetDatabase.Refresh();
        Debug.Log("Texture extraction and consolidation complete.");
    }

    private static void ExtractTexturesFromMaterial(Material material, string targetFolder)
    {
        Shader shader = material.shader;
        int propertyCount = ShaderUtil.GetPropertyCount(shader);

        for (int i = 0; i < propertyCount; i++)
        {
            if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
            {
                string propertyName = ShaderUtil.GetPropertyName(shader, i);
                Texture texture = material.GetTexture(propertyName);
                if (texture == null) continue;

                // Get the path of the texture
                string texturePath = AssetDatabase.GetAssetPath(texture);

                // Calculate the target path
                string targetPath = Path.Combine(targetFolder, Path.GetFileName(texturePath));

                // Check if a texture with the same name already exists
                if (!AssetDatabase.LoadAssetAtPath<Texture>(targetPath))
                {
                    AssetDatabase.CopyAsset(texturePath, targetPath);
                    Debug.Log($"Extracted: {texturePath} to {targetPath}");
                }
                else
                {
                    Debug.LogWarning($"Texture already exists: {targetPath}");
                }
            }
        }
    }
}

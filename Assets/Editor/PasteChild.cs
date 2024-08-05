using UnityEngine;
using UnityEditor;

public class PasteChildUtility : EditorWindow
{
    [MenuItem("Tools/Paste Child Into Selected Objects")]
    private static void PasteChild()
    {
        if (Selection.objects.Length == 0)
        {
            Debug.LogWarning("No objects selected. Please select at least one parent object.");
            return;
        }

        // Store the copied object
        GameObject copiedObject = null;

        // Check if a prefab is copied
        if (Selection.activeObject is GameObject && PrefabUtility.IsPartOfAnyPrefab(Selection.activeObject))
        {
            copiedObject = PrefabUtility.GetCorrespondingObjectFromOriginalSource(Selection.activeGameObject);
        }
        else if (Selection.activeObject is GameObject)
        {
            copiedObject = Selection.activeGameObject;
        }

        if (copiedObject == null)
        {
            Debug.LogWarning("No valid GameObject is copied. Please copy a GameObject to use this tool.");
            return;
        }

        // Iterate over each selected object and create a child copy
        foreach (GameObject parent in Selection.gameObjects)
        {
            GameObject newChild = (GameObject)PrefabUtility.InstantiatePrefab(copiedObject, parent.transform);
            newChild.name = copiedObject.name; // Ensure the child has the same name as the original
            newChild.transform.localPosition = Vector3.zero;
            newChild.transform.localRotation = Quaternion.identity;
            newChild.transform.localScale = Vector3.one;
        }

        Debug.Log($"Pasted '{copiedObject.name}' as a child into selected objects.");
    }
}

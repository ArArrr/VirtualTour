using UnityEngine;

public class MakeCombinedMeshReadable : MonoBehaviour
{
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null && meshFilter.mesh != null)
        {
            MakeMeshReadable(meshFilter.mesh);
        }

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            MeshFilter mf = renderer.GetComponent<MeshFilter>();
            if (mf != null && mf.mesh != null)
            {
                MakeMeshReadable(mf.mesh);
            }
        }
    }

    void MakeMeshReadable(Mesh mesh)
    {
        mesh.MarkDynamic();
        Debug.Log($"{mesh.name} is now readable.");
    }
}

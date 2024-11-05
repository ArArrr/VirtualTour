using UnityEngine;

public class spawnObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject SpawnObject;

    public void Spawn()
    {
        // Instantiate the SpawnObject as a child of the Canvas
        GameObject newObject = Instantiate(SpawnObject, Canvas.transform);

        // Set its local position to (0, 0, 0)
        newObject.transform.localPosition = Vector3.zero;
    }
}

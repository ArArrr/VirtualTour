using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;
    [SerializeField]
    private GameObject SpawnObjectPrefab;
    public int spawnIndex = 1;

    public void Spawn()
    {
        // Instantiate the SpawnObject as a child of the Canvas
        GameObject newObject = Instantiate(SpawnObjectPrefab, Canvas.transform);

        // Set its local position to (0, 0, 0)
        newObject.transform.localPosition = Vector3.zero;

        // Adjust the BoxCollider size in the Z-axis
        BoxCollider boxCollider = newObject.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Increase the Z size of the collider by spawnIndex
            Vector3 newSize = boxCollider.size;
            newSize.z += 1 * spawnIndex;
            boxCollider.size = newSize;
        }

        // Increment the spawn index for the next spawned object
        spawnIndex++;
    }
}

using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnPointID;  // Unique ID for the spawn point
    public Transform spawnLocation;  // Assign a position and rotation in the inspector

    public Vector3 GetSpawnPosition()
    {
        return spawnLocation.position;
    }

    public Quaternion GetSpawnRotation()
    {
        return spawnLocation.rotation;
    }
}

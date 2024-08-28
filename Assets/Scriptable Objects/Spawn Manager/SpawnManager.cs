using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public SpawnPoint selectedSpawnPoint;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SetSpawnPoint(SpawnPoint spawnPoint)
    {
        selectedSpawnPoint = spawnPoint;
    }

    public void SpawnPlayer(GameObject player)
    {
        if (selectedSpawnPoint != null)
        {
            player.transform.position = selectedSpawnPoint.GetSpawnPosition();
            player.transform.rotation = selectedSpawnPoint.GetSpawnRotation();
        }
    }
}

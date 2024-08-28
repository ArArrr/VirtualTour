using UnityEngine;

public class SceneStart : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        string targetID = DataManager.Instance.targetSpawnPointID;
        bool isFound = false;

        // Find the spawn point with the matching ID in the new scene
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnPointID.ToLower() == targetID.ToLower())
            {
                SpawnManager.Instance.SetSpawnPoint(spawnPoint);
                isFound = true;
                break;
            }
        }
        if (!isFound)
        {
            Debug.LogWarning("Spawn ID " + targetID + " not found. Teleporting to default.");
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                if (spawnPoint.spawnPointID == "default")
                {
                    SpawnManager.Instance.SetSpawnPoint(spawnPoint);

                    break;
                }
            }
        }

        // Spawn the player at the selected spawn point
        SpawnManager.Instance.SpawnPlayer(player);
    }
}

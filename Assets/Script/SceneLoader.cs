using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string targetSpawnPointID;
    public string sceneName; // Name of the scene to load
    public string transition = "CrossFade";
    public string soundEffect = "none";

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with a GameObject tagged as "Player" or any other tag you specify
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            if (!string.IsNullOrEmpty(targetSpawnPointID))
            {
                DataManager.Instance.targetSpawnPointID = targetSpawnPointID;
            }

            // Load the specified scene
            LevelManager.Instance.LoadScene(sceneName, transition, soundEffect);
        }
    }
}
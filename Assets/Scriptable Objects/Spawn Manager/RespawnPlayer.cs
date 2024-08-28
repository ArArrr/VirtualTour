using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour
{
    public SceneStart sceneStart;
    private void Start()
    {
        sceneStart = FindAnyObjectByType<SceneStart>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        // Check if the collision is with a GameObject tagged as "Player" or any other tag you specify
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            Debug.Log("Player went out of bounce. Teleporting to ID: "+DataManager.Instance.targetSpawnPointID);
            sceneStart.LoadScene();
        }
    }
}

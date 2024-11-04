    using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public GameObject block;            // The GameObject to enable/disable
    public bool toDestroy = false;      // Whether this script should self-destruct when nextLevel turns true
    public bool doNotUnblock = false;

    private void Start()
    {
        if (block.IsUnityNull()) block = gameObject;
        // Check the player's tour status and set the block active state accordingly
        if (DataManager.Instance.isTour)
        {
            block.SetActive(true);
            // Start listening for changes in the `nextLevel` variable
            if(!doNotUnblock) StartCoroutine(WaitForNextLevel());
        }
        else
        {
            block.SetActive(false);
        }
    }

    private IEnumerator WaitForNextLevel()
    {
        // Keep checking until `nextLevel` turns true
        while (!DataManager.Instance.nextLevel)
        {
            yield return new WaitForSeconds(0.2f); // Check every 0.1 seconds to reduce overhead
        }

        // If `nextLevel` turns true, handle based on `toDestroy` flag
        if (toDestroy)
        {
            // Destroy this GameObject and stop listening
            Destroy(block);
            Destroy(this);
        }
        else
        {
            // Simply disable the block if `toDestroy` is false
            block.SetActive(false);
        }
    }
}

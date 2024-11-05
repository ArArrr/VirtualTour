using UnityEngine;
using UnityEngine.UI;

public class GetUI : MonoBehaviour
{
    public GameObject ItemToGet;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the exact assigned prefab is the one that collided
        Debug.Log(other.gameObject.name + " detected.");
        if (other.gameObject.name.Contains(ItemToGet.name))
        {
            Destroy(other.gameObject); // Destroy the specific object instance
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1; // Make UI visible (or any effect you want)
            }

            // Disable the script after interaction
            this.enabled = false;
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GetUI : MonoBehaviour
{
    public GameObject ItemToGet;
    public UnityEvent events;
    public AudioSource source;
    public AudioClip clip;
    private void Start()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.4f; // Make UI visible (or any effect you want)
        }

    }
    private void OnDisable()
    {
        Debug.Log(gameObject.name + "'s item retrieved.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled)
        {
            // Check if the exact assigned prefab is the one that collided
            Debug.Log(other.gameObject.name + " detected.");
            if (other.gameObject.name.Contains(ItemToGet.name) || other == ItemToGet)
            {
                CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1; // Make UI visible (or any effect you want)
                }
                if (events != null)
                {
                    events.Invoke();
                    source.clip = clip;
                    source.Play();
                }
                // Disable the script after interaction
                Destroy(other.gameObject); // Destroy the specific object instance
                this.enabled = false;
            }
        }
    }
}

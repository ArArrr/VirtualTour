using TMPro;
using UnityEngine;

public class MarkerDistanceDisplay : MonoBehaviour
{
    [Header("NEXT NARRATION [Line] ")]
    public NarrationController nextNarration;  // Reference to the next narration controller
    public bool nextFloor = false;

    [Header("Customization")] 
    public Transform playerTransform;      // Reference to the player's transform
    public TextMeshProUGUI distanceText;   // Reference to the TextMeshPro UI component
    public float updateInterval = 0.2f;    // Time interval between updates in seconds
    public float baseScale = 1f;           // Base scale for the marker
    public float distanceMultiplier = 0.1f; // How much the scale should change with distance

    private Canvas canvas;

    private void Start()
    {
        // Automatically get the Canvas component from the GameObject
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        playerTransform = Camera.main.transform;
    }

    private void UpdateDistanceText()
    {
        if (playerTransform == null || distanceText == null)
        {
            Debug.LogWarning("Player Transform or Distance Text not assigned.");
            return;
        }

        // Calculate the distance from the player to the marker
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // Update the text with the distance (rounded to 1 decimal place)
        distanceText.text = $"{distance:F1} meters";

        // Adjust the scale of the marker based on distance
        AdjustMarkerScale(distance);
    }

    public void StartMarker()
    {
        canvas.enabled = true;
        InvokeRepeating(nameof(UpdateDistanceText), 0f, updateInterval);
        if(nextFloor && DataManager.Instance.isTour)
        {
            DataManager.Instance.nextLevel = true;
        }
    }

    private void AdjustMarkerScale(float distance)
    {
        // Calculate a new scale based on distance
        float newScale = baseScale + (distance * distanceMultiplier);

        // Apply the new scale to the marker (TextMeshPro GameObject)
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void OnDestroy()
    {
        // Ensure InvokeRepeating is canceled if the object is destroyed
        CancelInvoke(nameof(UpdateDistanceText));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the collider (assumes player has a specific tag)
        if (other.CompareTag("Player"))
        {
            if (nextNarration != null && canvas.enabled)
            {
                nextNarration.StartNarration();
                CancelInvoke(nameof(UpdateDistanceText));
                canvas.enabled = false;
            }
        }
    }
}

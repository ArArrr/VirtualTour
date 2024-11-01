using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MarkerDistanceDisplay : MonoBehaviour
{
    [Header("NEXT NARRATION [Line]")]
    public NarrationController nextNarration;  // Reference to the next narration controller
    public bool nextFloor = false;

    [Header("NEXT MARKERS [Markers]")]
    public List<MarkerDistanceDisplay> markers;  // List of next markers to trigger

    [Header("Customization")]
    public Transform playerTransform;      // Reference to the player's transform
    public TextMeshProUGUI distanceText;   // Reference to the TextMeshPro UI component
    public float updateInterval = 0.1f;    // Time interval between updates in seconds
    public float baseScale = 1f;           // Base scale for the marker
    public float distanceMultiplier = 0.1f; // How much the scale should change with distance
    public List<GameObject> outlinedObjects;

    private Canvas canvas;
    [SerializeField]
    public GameObject backup;

    private void Start()
    {
        if (!enabled) return;

        // Automatically get the Canvas component
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;

        // Attempt to find "Backup Marker" among children; create it if it doesn't exist
        Transform backupTransform = transform.Find("Backup Marker");
        if (backupTransform == null)
        {
            backup = Instantiate(backup);
            backup.transform.SetParent(transform);
            backup.transform.localPosition = new Vector3(0,18.6f,0);
            backup.transform.localScale = new Vector3(10, 10, 10);
            backup.transform.localRotation = Quaternion.Euler(90,0,0);

        }
        else
        {
            backup = backupTransform.gameObject;
        }
        backup.SetActive(false);

        // Cache player camera if not already assigned
        if (playerTransform == null)
        {
            playerTransform = Camera.main?.transform;
        }
    }

    private void UpdateDistanceText()
    {
        if (playerTransform == null || distanceText == null)
        {
            Debug.LogWarning("Player Transform or Distance Text not assigned.");
            return;
        }

        // Calculate distance and update the text with the distance
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        distanceText.text = $"{distance:F1} meters";

        // Adjust the scale of the marker based on distance
        AdjustMarkerScale(distance);
    }

    public void StartMarker()
    {
        canvas.enabled = true;
        backup?.SetActive(true);
        InvokeRepeating(nameof(UpdateDistanceText), 0f, updateInterval);

        // Check if the player is within range to trigger OnTriggerEnter
        Collider collider = GetComponent<Collider>();
        if (collider != null && playerTransform != null && collider.bounds.Contains(playerTransform.position))
        {
            OnTriggerEnter(collider); // Simulate player entering trigger if already inside
        }

        if (nextFloor && DataManager.Instance.isTour)
        {
            DataManager.Instance.nextLevel = true;
            DataManager.Instance.lastCompletedFloor++;
        }
    }


    private void AdjustMarkerScale(float distance)
    {
        // Calculate a new scale based on distance
        float newScale = baseScale + (distance * distanceMultiplier);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(UpdateDistanceText));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canvas.enabled)
        {
            nextNarration?.StartNarration();
            if (outlinedObjects != null) ApplyOutline();

            // Stop updating and hide the canvas
            CancelInvoke(nameof(UpdateDistanceText));
            canvas.enabled = false;
            backup?.SetActive(false);

            // Trigger all next markers
            foreach (MarkerDistanceDisplay marker in markers)
            {
                marker.StartMarker();
            }
        }
    }

    private void ApplyOutline()
    {
        if (outlinedObjects == null || outlinedObjects.Count == 0) return;

        foreach (GameObject obj in outlinedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline == null)
            {
                outline = obj.AddComponent<Outline>();
            }
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Required to use lists
using TMPro;

public class NarrationController : MonoBehaviour
{
    public bool isIntro = false;
    public SubtitleData subtitleData;  // Assign your subtitle data in the inspector
    private AudioSource audioSource;    // AudioSource for playing narration audio

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    [Header("Next Markers")]
    public List<MarkerDistanceDisplay> markers;  // List of markers to trigger

    [Header("Customization")]
    public float delayBeforeNext = 0f; // Optional delay before playing the next narration
    public bool waitAudioToFinish = true;

    [Header("Outline Settings")]
    public List<GameObject> outlinedObjects;  // List of objects to outline during narration
    public bool removeOutlineAfter = true;    // Should the outline be removed after the narration ends?

    private TMP_Text subtitleText;     // Reference to TMP_Text for subtitles
    private CanvasGroup subtitleCanvasGroup; // CanvasGroup to control visibility

    private void Start()
    {
        if (!enabled) return;  // Exit if this component was disabled by the parent

        // Proceed with the regular Start logic if enabled
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found in the parent object. Please make sure it exists.");
        }

        GameObject subtitleUI = GameObject.Find("Subtitle UI");
        if (subtitleUI != null)
        {
            subtitleCanvasGroup = subtitleUI.GetComponent<CanvasGroup>();
            subtitleText = subtitleUI.GetComponentInChildren<TMP_Text>();

            SetSubtitleVisible(false);
            if (isIntro) StartNarration();
        }
        else
        {
            Debug.LogError("Subtitle UI not found! Please make sure it exists in the scene.");
        }
    }

    public void StartNarration()
    {
        // Log the audio clip name when narration starts
        if (audioSource != null && audioSource.clip != null)
        {
            Debug.Log($"[ Line ] {audioSource.clip.name} is playing..");
        }
        else
        {
            Debug.LogWarning("[ Line ] No audio clip assigned or AudioSource is missing.");
        }

        ApplyOutline();  // Add outline to the specified objects

        StartCoroutine(PlayNarrationWithSubtitles());
    }

    private IEnumerator PlayNarrationWithSubtitles()
    {
        // Ensure there's an audio clip to play
        if (subtitleData == null || audioSource == null || subtitleData.audioClip == null)
        {
            Debug.LogError("SubtitleData or AudioSource is missing!");
            yield break;
        }
        if (isIntro) yield return new WaitForSeconds(4);
        SetSubtitleVisible(true);

        // Play the audio
        audioSource.clip = subtitleData.audioClip;
        audioSource.Play();

        foreach (var line in subtitleData.subtitleLines)
        {
            // Wait until it's time to show the next subtitle, relative to the audio
            while (audioSource.time < line.startTime)
            {
                yield return null; // Wait for the correct start time
            }

            // Show the subtitle
            subtitleText.text = line.text;
            SetSubtitleVisible(true);

            // Wait until the subtitle should end
            while (audioSource.time < line.endTime)
            {
                yield return null; // Wait for the correct end time
            }

            // Clear the subtitle
            subtitleText.text = "";
            SetSubtitleVisible(false);
        }

        // Wait until the entire audio clip is finished
        if (waitAudioToFinish) yield return new WaitForSeconds(audioSource.clip.length - subtitleData.subtitleLines[subtitleData.subtitleLines.Length - 1].endTime);

        // Hide the subtitle UI
        SetSubtitleVisible(false);

        RemoveOutline();  // Remove the outline if the option is enabled

        // Play the next narration, if available
        if (nextNarration != null)
        {
            yield return new WaitForSeconds(delayBeforeNext);  // Add delay before playing the next narration
            nextNarration.StartNarration();
        }

        // Start all markers in the list
        foreach (MarkerDistanceDisplay marker in markers)
        {
            if (marker != null)
            {
                marker.StartMarker();
            }
        }
    }

    // Set subtitle UI visibility using CanvasGroup
    private void SetSubtitleVisible(bool isVisible)
    {
        if (subtitleCanvasGroup != null)
        {
            subtitleCanvasGroup.alpha = isVisible ? 1 : 0;
        }
    }

    // Apply outline to all objects in the outlinedObjects list
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
        }
    }

    // Remove outline based on removeOutlineAfter flag
    private void RemoveOutline()
    {
        if (!removeOutlineAfter || outlinedObjects == null || outlinedObjects.Count == 0) return;

        foreach (GameObject obj in outlinedObjects)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                Destroy(outline);
            }
        }
    }
}

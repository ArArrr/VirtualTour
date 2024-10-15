using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NarrationController : MonoBehaviour
{
    public bool isIntro = false;
    public SubtitleData subtitleData;  // Assign your subtitle data in the inspector
    private AudioSource audioSource;    // AudioSource for playing narration audio

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    [Header("Next Marker")]
    public MarkerDistanceDisplay marker;

    [Header("Customization")]
    public float delayBeforeNext = 0f; // Optional delay before playing the next narration
    public bool waitAudioToFinish = true;

    private TMP_Text subtitleText;     // Reference to TMP_Text for subtitles
    private CanvasGroup subtitleCanvasGroup; // CanvasGroup to control visibility

    private void Start()
    {
        // Automatically find the AudioSource component from the parent object
        audioSource = GetComponentInParent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource not found in the parent object. Please make sure it exists.");
        }

        // Automatically find the Subtitle UI and its components
        GameObject subtitleUI = GameObject.Find("Subtitle UI");  // Find Subtitle UI by name
        if (subtitleUI != null)
        {
            subtitleCanvasGroup = subtitleUI.GetComponent<CanvasGroup>();
            subtitleText = subtitleUI.GetComponentInChildren<TMP_Text>();

            // Hide the subtitle UI at the start
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

        // Play the next narration, if available
        if (nextNarration != null)
        {
            yield return new WaitForSeconds(delayBeforeNext);  // Add delay before playing the next narration
            nextNarration.StartNarration();
        }
        if (marker != null)
        {
            marker.StartMarker();
        }
    }

    // Set subtitle UI visibility using CanvasGroup
    private void SetSubtitleVisible(bool isVisible)
    {
        if (subtitleCanvasGroup != null)
        {
            subtitleCanvasGroup.alpha = isVisible ? 1 : 0;
            //subtitleCanvasGroup.blocksRaycasts = isVisible;  // Optional: block raycasts only when visible
        }
    }
}

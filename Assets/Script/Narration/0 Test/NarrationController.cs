using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic; // Required to use lists
using TMPro;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;

public class NarrationController : MonoBehaviour
{
    [System.Serializable]
    public class DelayedUnityEvent
    {
        public UnityEvent unityEvent;
        public float delay; // Delay in seconds before triggering this specific event
    }

    public bool isIntro = false;
    public bool onlyOnce = false;
    private int played = 0;
    public SubtitleData subtitleData;  // Assign your subtitle data in the inspector
    private AudioSource audioSource;    // AudioSource for playing narration audio

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    [Header("Next Markers")]
    public List<MarkerDistanceDisplay> markers;  // List of markers to trigger

    [Header("Customization")]
    public float delayBeforeNext = 0f; // Optional delay before playing the next narration
    public bool waitAudioToFinish = true;
    public List<GameObject> ActivateObject;
    public List<GameObject> DeactivateObject;

    [Header("Outline Settings")]
    public List<GameObject> outlinedObjects;  // List of objects to outline during narration
    public bool removeOutlineAfter = true;    // Should the outline be removed after the narration ends?
    public bool OnlyOutlineAfter = false;    // Should the outline be removed after the narration ends?

    private TMP_Text subtitleText;     // Reference to TMP_Text for subtitles
    private CanvasGroup subtitleCanvasGroup; // CanvasGroup to control visibility

    [Header("Invoke Event at the Start")]
    public List<DelayedUnityEvent> delayedEvents;

    [Header("Invoke Event at the End")]
    public UnityEvent onCustomEventTriggered2;

    private void Start()
    {
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
        if (!enabled) return;  // Exit if this component was disabled by the parent
    }

    public void StartNarration()
    {
        if (onlyOnce && played >= 1) return;
        if (onlyOnce) played++;
        // Log the audio clip name when narration starts
        if (audioSource != null && audioSource.clip != null)
        {
            Debug.Log($"[ Line ] {audioSource.clip.name} is playing..");
        }
        else
        {
            Debug.LogWarning(gameObject.name + " No audio clip assigned or AudioSource is missing.");
        }

        if (!OnlyOutlineAfter)
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

        TriggerCustomEvent();

        const float timeTolerance = 0.05f;  // Small tolerance to prevent looping issue
        foreach (var line in subtitleData.subtitleLines)
        {
            float adjustedEndTime = Mathf.Min(line.endTime, audioSource.clip.length);

            // Wait until it's time to show the next subtitle, relative to the audio
            while (audioSource.time < line.startTime)
            {
                yield return null; // Wait for the correct start time
            }

            // Show the subtitle
            subtitleText.text = line.text;
            SetSubtitleVisible(true);

            // Wait until the subtitle should end, with tolerance
            while (audioSource.time < adjustedEndTime - timeTolerance)
            {
                yield return null;  // Wait for the correct end time
            }

            // Clear the subtitle
            subtitleText.text = "";
            SetSubtitleVisible(false);
        }

        // Wait until the entire audio clip is finished
        if (waitAudioToFinish)
        {
            float secondsLeft = audioSource.clip.length - subtitleData.subtitleLines[subtitleData.subtitleLines.Length - 1].endTime;
            Debug.Log("Waiting for audio to finish. "+secondsLeft+" seconds.");
            yield return new WaitForSeconds(secondsLeft);
        }

        // Hide the subtitle UI
        SetSubtitleVisible(false);


        if (!OnlyOutlineAfter) RemoveOutline();  // Remove the outline if the option is enabled
        if (OnlyOutlineAfter) ApplyOutline();

        // Play the next narration, if available
        if (nextNarration != null)
        {
            yield return new WaitForSeconds(delayBeforeNext);  // Add delay before playing the next narration
            nextNarration.StartNarration();
        }

        TriggerCustomEvent2();

        // Start all markers in the list
        foreach (MarkerDistanceDisplay marker in markers)
        {
            if (marker != null)
            {
                marker.StartMarker();
            }
        }

        foreach (GameObject obj in ActivateObject)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        foreach (GameObject obj in DeactivateObject)
        {
            if (obj != null)
            {
                obj.SetActive(false);
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
                outline.OutlineMode = Outline.Mode.OutlineAll;
            } else
            {
                outline.enabled = true;
                outline.OutlineMode = Outline.Mode.OutlineAll;
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
            //if (outline != null)
            //{
            //    outline.OutlineMode = Outline.Mode.OutlineHidden;
            //}

            outline.enabled = false;
        }
    }
    // Call this method when the event is triggered
    public void TriggerCustomEvent()
    {
        StartCoroutine(InvokeWithDelays());
    }

    // Coroutine to invoke each event with its associated delay
    private IEnumerator InvokeWithDelays()
    {
        float lastDelay = 0;
        float eventDelay;
        float newDelay;
        int index = 0;
        foreach (var delayedEvent in delayedEvents)
        {
            eventDelay = delayedEvent.delay;
            if(lastDelay == 0) newDelay = eventDelay;
            else newDelay =  eventDelay - lastDelay;
            // Wait for the specified delay
            Debug.Log("Event " + index + " will play in " + newDelay + " seconds.");
            yield return new WaitForSeconds(newDelay);
            lastDelay = delayedEvent.delay;
            
            // Invoke the UnityEvent
            delayedEvent.unityEvent.Invoke();
            Debug.Log("Playing Event "+index+"...");
            index++;
        }
    }

    public void TriggerCustomEvent2()
    {
        // Invoke the event (calls all assigned methods)
        if (onCustomEventTriggered2 != null)
        {
            onCustomEventTriggered2.Invoke();
        }
    }
}

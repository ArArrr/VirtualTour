using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OvenDial : DistanceReleaseGrabInteractable
{
    public Light ovenLight;                     // Reference to the light in the oven
    public AudioSource ovenAudioSource;         // Audio source for playing sounds
    public AudioClip turnDial;
    public AudioClip turnOnAudio;               // Audio to play when the dial starts turning
    public AudioClip turnOffAudio;              // Audio to play when the light turns off
    public AudioSource ovenFryingSource;        // Frying sound source
    public AudioClip fryingSound;               // Frying sound clip
    public float transitionDuration = 2f;       // Duration for the dial rotation

    private bool isRotating = false;            // Is the dial rotating?
    private Quaternion startRotation;           // Initial rotation of the dial
    private Quaternion targetRotation;          // Final rotation (Y 90 degrees)

    public GameObject dial;                     // Reference to the dial GameObject

    public GameObject nextItem;
    public GameObject currentItem;

    private void Start()
    {
        // Add listener for the grab event
        selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!isRotating)
        {
            isRotating = true;
            DetachInteractor();  // Detach the grab interaction

            // Set up the initial and target rotations
            startRotation = dial.transform.rotation;
            targetRotation = Quaternion.Euler(dial.transform.eulerAngles.x, 90f, dial.transform.eulerAngles.z);

            ovenAudioSource.clip = turnDial;
            ovenAudioSource.Play();
            StartCoroutine(RotateDial()); // Start rotating the dial
        }
    }

    private IEnumerator RotateDial()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            //// Smoothly rotate the dial from the startRotation to targetRotation over time
            //dial.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final rotation is exactly Y 90 degrees
        dial.transform.rotation = targetRotation;

        ovenLight.enabled = true; // Turn on the oven light

        ovenAudioSource.clip = turnOnAudio;
        ovenAudioSource.Play(); // Play turn on audio

        ovenFryingSource.clip = fryingSound;
        ovenFryingSource.Play();

        // Wait for 5 seconds, then turn off the light and play turn off audio
        yield return new WaitForSeconds(8f);
        ovenLight.enabled = false;
        ovenAudioSource.clip = turnOffAudio;
        ovenAudioSource.Play();
        ovenFryingSource.Stop(); // Stop frying sound

        // Disable the oven dial script to prevent further interaction
        Outline outline = gameObject.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = false;
        }
        this.enabled = false;

        nextItem.SetActive(true);
        currentItem.SetActive(false);
    }
}

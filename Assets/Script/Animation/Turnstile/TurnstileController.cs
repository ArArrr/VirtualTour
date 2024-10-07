using UnityEngine;

public class TurnstileController : MonoBehaviour
{
    [Header("Animator Reference")]
    public Animator turnstileAnimator;

    [Header("Audio Clips")]
    public AudioClip cardReadClip;        // Audio clip to play when the card is read
    public AudioClip playerEnterClip;     // Audio clip to play when the player enters the wheel

    private bool isCardRead = false;
    private bool isOutside = false;

    private AudioSource audioSource;      // Reference to the AudioSource component

    private void Start()
    {
        // Get the AudioSource component attached to the same GameObject
        audioSource = GetComponent<AudioSource>();

        // Ensure the AudioSource exists
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found. Please attach an AudioSource to the Turnstile object.");
        }
    }

    // This function gets called when an ID Card is read
    public void OnCardRead(bool isEntryReader)
    {
        isCardRead = true;
        isOutside = isEntryReader;

        // Update the animator parameters based on card interaction
        turnstileAnimator.SetBool("isCardRead", isCardRead);
        turnstileAnimator.SetBool("isOutside", isOutside);
        turnstileAnimator.SetTrigger("Wait");

        // Play the card read audio clip
        PlayAudioClip(cardReadClip);

        Debug.Log("Card Read: " + (isOutside ? "IN" : "OUT"));
    }

    // This function gets called when the player enters the wheel collider
    public void OnPlayerEnteredWheel()
    {
        if (isCardRead)
        {
            // Trigger the rotation animation
            turnstileAnimator.SetTrigger("Turn");

            // Play the player enter audio clip
            PlayAudioClip(playerEnterClip);

            // Reset values after the animation completes
            Invoke("ResetTurnstile", 1.1f); // Assuming animation length is 2 seconds
        }
    }

    // Resets the turnstile to its initial state after animation
    public void ResetTurnstile()
    {
        isCardRead = false;
        isOutside = false;
        turnstileAnimator.SetBool("isCardRead", isCardRead);
        turnstileAnimator.SetBool("isOutside", isOutside);

        Debug.Log("Turnstile reset to waiting state.");
    }

    // Helper function to play the provided audio clip
    private void PlayAudioClip(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Play the clip using PlayOneShot to avoid interruptions
        }
    }
}

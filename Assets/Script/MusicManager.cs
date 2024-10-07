using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public AudioSource audioSource; // Reference to the AudioSource component

    private void Awake()
    {
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Avoid duplicate MusicManagers
            return;
        }

        // Make the object persistent across scenes
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("MusicManager is missing from the scene!");
            }
            return instance;
        }
    }

    // Helper method to play a specific music clip
    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop any currently playing music
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    // New helper method to stop the current music
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

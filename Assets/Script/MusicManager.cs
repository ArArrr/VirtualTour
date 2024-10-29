using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    public AudioSource audioSource; // Reference to the AudioSource component
    public List<AudioClip> MusicPlaylist;
    public bool autoPlay = true;
    public int currentlyPlaying = 0;
    
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
    private void Update()
    {
        if (autoPlay && !audioSource.isPlaying)
        {
            StartCoroutine(nextMusic());
        }
    }

    public void NextMusic()
    {
        // Advance to the next clip, loop back to the start if at the end of the playlist
        currentlyPlaying = (currentlyPlaying + 1) % MusicPlaylist.Count;
        PlayMusic(MusicPlaylist[currentlyPlaying]);
    }


    public IEnumerator nextMusic()
    {
        yield return new WaitForSeconds(1f);
        NextMusic();
        yield break;
    }
}

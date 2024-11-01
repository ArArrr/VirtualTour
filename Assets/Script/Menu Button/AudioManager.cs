using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;          // Reference to your Audio Mixer
    public Slider masterVolumeSlider;      // Master volume slider
    public Slider musicVolumeSlider;       // Music volume slider

    private void Start()
    {
        // Set sliders to DataManager's saved values
        masterVolumeSlider.value = DataManager.Instance.masterVolume;
        musicVolumeSlider.value = DataManager.Instance.musicVolume;

        // Update Audio Mixer with initial DataManager volume values
        SetMasterVolume(DataManager.Instance.masterVolume);
        SetMusicVolume(DataManager.Instance.musicVolume);

        // Add listeners for slider value changes
        masterVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SetMasterVolume(value);
            DataManager.Instance.masterVolume = value; // Save new value
        });

        musicVolumeSlider.onValueChanged.AddListener((value) =>
        {
            SetMusicVolume(value);
            DataManager.Instance.musicVolume = value; // Save new value
        });
    }

    // Set the master volume
    public void SetMasterVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat("Master", -80f); // Set to silence
        }
        else
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20); // Convert to decibel scale
        }
    }

    // Set the music volume
    public void SetMusicVolume(float volume)
    {
        if (volume == 0)
        {
            audioMixer.SetFloat("Music", -80f); // Set to silence
        }
        else
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20); // Convert to decibel scale
        }
    }
}

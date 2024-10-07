using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ElevatorAudioController : MonoBehaviour
{
    [Header("Properties")]
    public int floor = 1;
    public int elevatorSet = 1;
    public string spawnID = "F1Elevator1";
    public TextMeshProUGUI floorText;

    [Header("Audio Clips")]
    [SerializeField] AudioClip ding;
    [SerializeField] AudioClip open;
    [SerializeField] AudioClip close;
    [SerializeField] AudioClip noise;
    public AudioClip[] music;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        ChangeFloor(floor);
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeFloor(int floor)
    {
        switch(floor)
        {
            case 0:
                floorText.text = "B";
                break;
            case 1:
                floorText.text = "G";
                break;
            case 2:
                floorText.text = "2";
                break;
            case 3:
                floorText.text = "3";
                break;
            case 4:
                floorText.text = "4";
                break;
            case 5:
                floorText.text = "5";
                break;
            case 6:
                floorText.text = "6";
                break;
            case 7:
                floorText.text = "7";
                break;
            case 8:
                floorText.text = "8";
                break;
        }
        Debug.Log("Current floor: "+floorText.text);
    }
    public void PlayDing()
    {
        audioSource.clip = ding;
        audioSource.Play();
    }
    public void PlayNoise()
    {
        audioSource.clip = noise;
        audioSource.Play();
    }

    public void PlayOpen()
    {
        audioSource.clip = open;
        audioSource.Play();
    }
    public void PlayClose()
    {
        audioSource.clip = close;
        audioSource.Play();
    }
    public void PlayMusic()
    {
        if (music.Length > 0)
        {
            // Get a random index from the music array
            int randomIndex = Random.Range(0, music.Length);
            if (!music[randomIndex].IsUnityNull())
            {
                MusicManager.Instance.audioSource.clip = music[randomIndex];
                MusicManager.Instance.audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Music array is empty. No music to play.");
        }
    }
}

using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ElevatorButtonController : MonoBehaviour
{
    public ElevatorAudioController elevatorAudio; // Reference to the ElevatorAudioController
    public Animator elevatorAnimator;             // Reference to the Animator
    public Renderer elevatorRenderer;             // Reference to the Renderer of the elevator

    [Header("Audio")]
    public AudioSource buttonAudio;
    public AudioClip click;

    [Header("Material")]
    public Material originalMaterial;             // Original material to revert to
    public Material highlightMaterial;            // Material to use during the delay

    [Header("Customize Floor")]
    public int targetFloor = 1;
    public TextMeshProUGUI floortext;

    private string targetScene;
    private string targetSpawnID;
    public GameObject lib;
    public GameObject libOut;

    private void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
        elevatorRenderer = GetComponent<Renderer>();
        getFloorID();
    }

    // Method to go to the assigned floor
    public void GoToFloor()
    {
        PlayClick();
        if (!ButtonShouldWork()) return; // Check if button should function
        if (!elevatorAnimator.GetBool("isWaiting")) StartCoroutine(HandleFloorChange());
    }

    // Method to call the elevator to the current floor
    public void CallElevator()
    {
        PlayClick();
        if (!ButtonShouldWork()) return; // Check if button should function
        if (elevatorAudio == null || elevatorAnimator == null || elevatorRenderer == null)
        {
            Debug.LogWarning("ElevatorAudioController, Animator, or Renderer is not assigned!");
            return;
        }

        if (!elevatorAnimator.GetBool("isWaiting") && !elevatorAnimator.GetBool("isOpen"))
            StartCoroutine(HandleCallElevator());
    }

    // Play the click sound
    public void PlayClick()
    {
        buttonAudio.clip = click;
        buttonAudio.Play();
    }

    // Open the elevator doors
    public void OpenElevator()
    {
        if (!elevatorAnimator.GetBool("isOpen") && !elevatorAnimator.GetBool("isWaiting"))
        {
            Debug.Log("Elevator door: Open");
            elevatorAudio.PlayOpen();
            elevatorAnimator.SetTrigger("Open");
            elevatorAnimator.SetBool("isOpen", true);
            //lib.SetActive(false);
            //libOut.SetActive(false);
        }
    }

    // Close the elevator doors
    public void CloseElevator()
    {
        if (elevatorAnimator.GetBool("isOpen") && !elevatorAnimator.GetBool("isWaiting"))
        {
            Debug.Log("Elevator door: Close");
            elevatorAudio.PlayClose();
            elevatorAnimator.SetTrigger("Close");
            elevatorAnimator.SetBool("isOpen", false);
        }
    }

    private IEnumerator HandleCallElevator()
    {
        Debug.Log("fun: "+DataManager.Instance.funValue);
        if (DataManager.Instance.funValue == 5 && lib != null && elevatorAudio.floor == 5 && targetFloor == -1)
        {
            Debug.Log("lib is here");
            lib.SetActive(true);
        }
        DataManager.Instance.rollFun();
        elevatorAnimator.SetBool("isWaiting", true);
        elevatorRenderer.material = highlightMaterial;
        yield return new WaitForSeconds(1);

        elevatorAudio.PlayNoise();

        float randomDelay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randomDelay);

        if (floortext != null) floortext.text = elevatorAudio.floorText.text;

        elevatorAudio.PlayDing();
        yield return new WaitForSeconds(2);

        Debug.Log($"Elevator button pressed. Waited {randomDelay} seconds before opening.");
        elevatorRenderer.material = originalMaterial;
        elevatorAnimator.SetBool("isWaiting", false);

        OpenElevator();
        yield return new WaitForSeconds(3);
        lib.SetActive(false);
    }

    private IEnumerator HandleFloorChange()
    {
        int trgFloor = targetFloor;
        int currentFloor = elevatorAudio.floor;
        int update = 0;

        if (currentFloor != trgFloor)
        {
            CloseElevator();
            elevatorRenderer.material = highlightMaterial;
            elevatorAnimator.SetBool("isWaiting", true);
            yield return new WaitForSeconds(1);

            if (currentFloor < trgFloor) update = 1;
            else if (currentFloor > trgFloor) update = -1;

            if (update != 0)
            {
                elevatorAudio.PlayNoise();
                elevatorAudio.PlayMusic();
                if (update == 1)
                {
                    for (int f = currentFloor; f <= targetFloor; f += update)
                    {
                        elevatorAudio.ChangeFloor(f);
                        if (f == 5 && DataManager.Instance.funValue == 13)
                        {
                            Debug.Log("lib is here");
                            elevatorAudio.audioSource.Stop();
                            MusicManager.Instance.audioSource.Pause();
                            yield return new WaitForSeconds(1);
                            elevatorAudio.audioSource.clip = elevatorAudio.noise3;
                            //buttonAudio.clip = elevatorAudio.noise2;
                            //buttonAudio.Play();
                            elevatorAudio.audioSource.Play();
                            yield return new WaitForSeconds(10);
                            buttonAudio.Stop();
                            //elevatorAudio.audioSource.Stop();
                            yield return new WaitForSeconds(2);
                            elevatorAudio.PlayNoise();
                            MusicManager.Instance.audioSource.Play();
                        }
                        yield return new WaitForSeconds(2);
                    }
                }
                else
                {
                    for (int f = currentFloor; f <= targetFloor; f += update)
                    {
                        elevatorAudio.ChangeFloor(f);
                        if (f == 5 && DataManager.Instance.funValue == 13)
                        {
                            Debug.Log("lib is here");
                            elevatorAudio.audioSource.Stop();
                            MusicManager.Instance.audioSource.Pause();
                            yield return new WaitForSeconds(1);
                            elevatorAudio.audioSource.clip = elevatorAudio.noise3;
                            //buttonAudio.clip = elevatorAudio.noise2;
                            //buttonAudio.Play();
                            elevatorAudio.audioSource.Play();
                            yield return new WaitForSeconds(10);
                            buttonAudio.Stop();
                            //elevatorAudio.audioSource.Stop();
                            yield return new WaitForSeconds(2);
                            elevatorAudio.PlayNoise();
                            MusicManager.Instance.audioSource.Play();
                        }
                        yield return new WaitForSeconds(2);
                    }
                }
            }

            elevatorRenderer.material = originalMaterial;
            setTargetSpawnID();
            elevatorAnimator.SetBool("isWaiting", false);
            if (DataManager.Instance.nextLevel)
            {
                DataManager.Instance.nextLevel = false;
            }
            LevelManager.Instance.LoadScene(targetScene, "none", "none");

        }
        else
        {
            elevatorAnimator.SetBool("isWaiting", true);
            elevatorRenderer.material = highlightMaterial;
            yield return new WaitForSeconds(1);
            elevatorRenderer.material = originalMaterial;
            elevatorAnimator.SetBool("isWaiting", false);
        }
    }

    public IEnumerator HandleSpawnElevator()
    {
        Debug.Log("fun: " + DataManager.Instance.funValue);
        if (DataManager.Instance.targetSpawnPointID == "F5Elevator1" && DataManager.Instance.funValue == 26)
        {
            Debug.Log("lib is here");
            libOut.SetActive(true);
        }
        DataManager.Instance.rollFun();
        elevatorAnimator.SetBool("isWaiting", false);
        //Debug.Log("isWaiting set to false");
        elevatorAudio.PlayDing();
        //Debug.Log("ding played");
        yield return new WaitForSeconds(2);
        //Debug.Log("2 seconds passed");
        MusicManager.Instance.StopMusic();
        //Debug.Log("music stopped");
        OpenElevator();
        //Debug.Log("elevator opened");
        yield return new WaitForSeconds(3);
        libOut.SetActive(false);
    }

    private void getFloorID()
    {
        switch (targetFloor)
        {
            case 0: targetScene = "BF Canteen"; break;
            case 1: targetScene = "1F Lobby"; break;
            case 2: targetScene = "2FR Hallway"; break;
            case 3: targetScene = "3F Hallway"; break;
            case 4: targetScene = "4F Hallway"; break;
            case 5: targetScene = "5F Hallway"; break;
            case 6: targetScene = "6F Hallway"; break;
            case 7: targetScene = "7F Hallway"; break;
            case 8: targetScene = "8F Court"; break;
        }
        targetSpawnID = "F" + targetFloor + "Elevator" + elevatorAudio.elevatorSet.ToString();
    }

    private void setTargetSpawnID()
    {
        if (!string.IsNullOrEmpty(targetSpawnID))
        {
            DataManager.Instance.targetSpawnPointID = targetSpawnID;
        }
    }

    // Check if the button should function based on tour conditions
    private bool ButtonShouldWork()
    {
        if (!DataManager.Instance.isTour) return true; // If not on a tour, all buttons work
        if (DataManager.Instance.isTour && DataManager.Instance.nextLevel && targetFloor == matchFloorButton()) return true; // On tour, only 8th floor works
        if (targetFloor == -1) return true;
        return false; // All other cases, button should not work
    }

    private int matchFloorButton()
    {
        int playerTargetFloor = DataManager.Instance.lastCompletedFloor;
        switch(playerTargetFloor)
        {
            case 0: return -1;
            case 1: return 0;
            case 2: return 8;
            case 3: return 7;
            case 4: return 6;
            case 5: return 5;
            case 6: return 4;
            case 7: return 3;
            case 8: return 2;
            case 9: return 1;
            default: return -1;
        }
    }
}

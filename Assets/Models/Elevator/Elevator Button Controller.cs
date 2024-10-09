using System.Collections;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
        elevatorRenderer = GetComponent<Renderer>();
        getFloorID();
    }

    // Method to go to the assigned floor
    public void GoToFloor()
    {
        if (!ButtonShouldWork()) return; // Check if button should function
        PlayClick();

        if (!elevatorAnimator.GetBool("isWaiting")) StartCoroutine(HandleFloorChange());
    }

    // Method to call the elevator to the current floor
    public void CallElevator()
    {
        if (!ButtonShouldWork()) return; // Check if button should function
        PlayClick();

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
                        yield return new WaitForSeconds(2);
                    }
                }
                else
                {
                    for (int f = currentFloor; f >= targetFloor; f += update)
                    {
                        elevatorAudio.ChangeFloor(f);
                        yield return new WaitForSeconds(2);
                    }
                }
            }

            elevatorRenderer.material = originalMaterial;
            setTargetSpawnID();
            elevatorAnimator.SetBool("isWaiting", false);
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
        elevatorAnimator.SetBool("isWaiting", false);
        Debug.Log("isWaiting set to false");
        elevatorAudio.PlayDing();
        Debug.Log("ding played");
        yield return new WaitForSeconds(2);
        Debug.Log("2 seconds passed");
        MusicManager.Instance.StopMusic();
        Debug.Log("music stopped");
        OpenElevator();
        Debug.Log("elevator opened");
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
        if (DataManager.Instance.isTour && DataManager.Instance.nextLevel && targetFloor == 8) return true; // On tour, only 8th floor works
        return false; // All other cases, button should not work
    }
}

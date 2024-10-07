using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
    public Material highlightMaterial;            // Material to use during the dela

    [Header("Customize Floor")]
    public int targetFloor = 1;
    public TextMeshProUGUI floortext;

    private string targetScene;
    private string targetSpawnID;

    // Method to call when the button is pressed
    private void Start()
    {
        buttonAudio = GetComponent<AudioSource>();
        elevatorRenderer = GetComponent<Renderer>();
        getFloorID();
    }

    public void GoToFloor()
    {
        PlayClick();
        if (!elevatorAnimator.GetBool("isWaiting")) StartCoroutine(HandleFloorChange());
    }

    public void PlayClick()
    {
        buttonAudio.clip = click;
        buttonAudio.Play();
    }

    public void CallElevator()
    { 
        if (elevatorAudio == null || elevatorAnimator == null || elevatorRenderer == null)
        {
            Debug.LogWarning("ElevatorAudioController, Animator, or Renderer is not assigned!");
            return;
        }
        PlayClick();

        // Start the process: Play noise, wait, and then play ding and open the door
        if (!elevatorAnimator.GetBool("isWaiting") && !elevatorAnimator.GetBool("isOpen"))
        StartCoroutine(HandleCallElevator());
    }

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

        // Step 1: Play the elevator noise sound
        elevatorAudio.PlayNoise();

        // Step 2: Wait for a random time between 2 to 5 seconds
        float randomDelay = Random.Range(2f, 5f);
        yield return new WaitForSeconds(randomDelay);

        if (!floortext.IsUnityNull()) floortext.text = elevatorAudio.floorText.text;
        // Step 3: Play the ding sound
        elevatorAudio.PlayDing();
        yield return new WaitForSeconds(2);

        // Optional: Debug log for verification
        Debug.Log($"Elevator button pressed. Waited {randomDelay} seconds before opening.");
        elevatorRenderer.material = originalMaterial;
        elevatorAnimator.SetBool("isWaiting", false);

        OpenElevator();
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
                    for (int f = currentFloor; f <= targetFloor;)
                    {
                        elevatorAudio.ChangeFloor(f);
                        yield return new WaitForSeconds(2);
                        f += update;
                    }
                }
                else
                {
                    for (int f = currentFloor; f >= targetFloor;)
                    {
                        elevatorAudio.ChangeFloor(f);
                        yield return new WaitForSeconds(2);
                        f += update;
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
    private void getFloorID()
    {
        switch (targetFloor)
        {
            case 0:
                targetScene = "BF Canteen";
                targetSpawnID = "F0Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 1:
                targetScene = "1F Lobby";
                targetSpawnID = "F1Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 2:
                targetScene = "2FR Hallway";
                targetSpawnID = "F2Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 3:
                targetScene = "3F Hallway";
                targetSpawnID = "F3Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 4:
                targetScene = "4F Hallway";
                targetSpawnID = "F4Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 5:
                targetScene = "5F Hallway";
                targetSpawnID = "F5Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 6:
                targetScene = "6F Hallway";
                targetSpawnID = "F6Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 7:
                targetScene = "7F Hallway";
                targetSpawnID = "F7Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
            case 8:
                targetScene = "8F Court";
                targetSpawnID = "F8Elevator" + elevatorAudio.elevatorSet.ToString();
                break;
        }
    }
    private void setTargetSpawnID()
    {
        if (!string.IsNullOrEmpty(targetSpawnID))
        {
            DataManager.Instance.targetSpawnPointID = targetSpawnID;
        }
    }
}

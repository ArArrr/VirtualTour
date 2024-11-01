using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UseCamera : MonoBehaviour
{
    public GameObject cameraObj;
    public GameObject flash;
    public AudioSource audioSource;
    public AudioClip clip;
    private Animator animator;
    public bool photoTaken;
    private bool wait = false;
    private bool isBeingHeld = false;
    private Outline outline;
    public TextMeshProUGUI countTaken;
    public Button accept;
    public Button cancel;

    [Header("Narration List")]
    public NarrationController Explain;
    public NarrationController narration1;
    public NarrationController narration2;
    public NarrationController narration3;
    public NarrationController great;
    public NarrationController doesItLookGood;
    public NarrationController Outro;
    public bool isNarrating = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cameraObj.SetActive(false);
        outline = GetComponent<Outline>();

        if (DataManager.Instance.cameraInUse)
        {
            GameObject devSim = GameObject.Find("XR Device Simulator");
            if(devSim != null )
            {
                PlayerInteraction playerInteraction = devSim.GetComponent<PlayerInteraction>();
                if (playerInteraction != null && playerInteraction.enabled == true)
                {
                    playerInteraction?.getCamera();
                }
            }
        }
        updateText();
    }

    private void OnDestroy()
    {
        // Stop all coroutines and unbind input actions
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        // Disable input actions and unbind to avoid memory leaks
    }

    public void ClickInteraction()
    {
        if (!wait && outline.enabled && !DataManager.Instance.isInMenu && DataManager.Instance.introDone && !isNarrating) StartCoroutine(openPicture());
    }

    public void confirm()
    {
        accept.interactable = false;
        DataManager.Instance.picCount++;
        if (DataManager.Instance.picCount != 4) updateText();
        StartCoroutine(closePicture());
        switch(DataManager.Instance.picCount)
        {
            case 2: narration2.StartNarration(); break;
            case 3: narration3.StartNarration(); break;
            case 4: Outro.StartNarration(); break;
        }
    }

    public void deny()
    {
        cancel.interactable = false;
        StartCoroutine(closePicture());
    }
    public void setNarration(bool b)
    {
        isNarrating = b;
    }
    public void setIsIntro(bool b)
    {
        DataManager.Instance.introDone = b;
    }
    public void setFirstShot(bool b)
    {
        DataManager.Instance.firstShot = b;
    }
    public void startExplain()
    {
        if (!DataManager.Instance.introDone)
        {
            Explain.StartNarration();
        }
        
    }

    public IEnumerator openPicture()
    {
        if (photoTaken == false)
        {
            if (this == null) yield break; // Check if the object has been destroyed

            cameraObj.SetActive(true);
            flash.SetActive(true);
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(0.2f);

            if (this == null) yield break;

            cameraObj.SetActive(false);
            flash.SetActive(false);
            photoTaken = true;
            animator.SetTrigger("Next");
            yield return new WaitForSeconds(1f);
            RandomCompliment();

            yield break;
        }
    }

    public IEnumerator closePicture()
    {
        if (this == null) yield break;

        animator.SetTrigger("Next");
        yield return new WaitForSeconds(2f);
        wait = false;
        photoTaken = false;
        accept.interactable = true;
        cancel.interactable = true;
        yield break;
    }

    public void updateText()
    {
        string text = DataManager.Instance.picCount + " / 3";
        countTaken.text = text;
    }

    public void RandomCompliment()
    {
        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 0)
        {
            doesItLookGood.StartNarration();
        }
        else
        {
            great.StartNarration();
        }
    }
    public void teleportToLobby()
    {
        DataManager.Instance.targetSpawnPointID = "Lobby";
        LevelManager.Instance.LoadScene("1F Lobby", "CrossFade", "none");
    }
}

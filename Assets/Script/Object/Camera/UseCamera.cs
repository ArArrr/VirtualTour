using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseCamera : MonoBehaviour
{
    public InputActionReference mouseClick;
    public GameObject cameraObj;
    public GameObject flash;
    public AudioSource audioSource;
    public AudioClip clip;
    private Animator animator;
    public bool photoTaken;
    private bool wait = false;
    private bool isBeingHeld = false;
    private Outline outline;

    private void Start()
    {
        mouseClick.action.performed += context => ClickInteraction();
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
    }

    private void OnDestroy()
    {
        // Stop all coroutines and unbind input actions
        StopAllCoroutines();
        mouseClick.action.Disable();
        mouseClick.action.performed -= context => ClickInteraction();
    }

    private void OnDisable()
    {
        // Disable input actions and unbind to avoid memory leaks
        mouseClick.action.Disable();
    }

    public void ClickInteraction()
    {
        if (!wait && outline.enabled && !DataManager.Instance.isInMenu) StartCoroutine(InvokeWithDelays());
    }

    private IEnumerator InvokeWithDelays()
    {
        wait = true;

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
        }
        else
        {
            if (this == null) yield break;

            animator.SetTrigger("Next");
            yield return new WaitForSeconds(2f);
            photoTaken = false;
        }

        wait = false;
        yield break;
    }
}

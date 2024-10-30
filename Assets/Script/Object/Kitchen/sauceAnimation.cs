using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class sauceAnimation : MonoBehaviour
{
    public Transform sauceAnchor;
    public Vector3 targetScale = new Vector3(1f, 1f, 1f);
    public float scalingTime = 3f;
    private float currentScalingTime = 0f;
    private Vector3 initialScale;

    public bool isPouring = false;
    public bool isInsideCollider = false;

    public float minTiltAngle = 45f;
    public float maxTiltAngle = 180f;

    public GameObject sauceBowl;
    public GameObject sauceBowlModel;
    public Outline sauceOutline;
    public MeshRenderer sauceRenderer;
    public ParticleSystem doughParticles;

    public GameObject nextItem;
    public Material transparentMaterial;

    private Outline nextItemOutline;
    public Outline nextItemOutline2;

    public XRGrabInteractable grabInteractable;  // XR Grab Interactable component

    [Header("Next Narration")]
    public NarrationController nextNarration;  // Reference to the next narration controller

    private void Start()
    {
        doughParticles.Stop();
        initialScale = sauceAnchor.localScale;
        if (sauceOutline != null)
        {
            sauceOutline.enabled = true;
        }

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // When grabbed, get the instance material from the sauceBowlModel's MeshRenderer
        sauceRenderer = sauceBowlModel.GetComponent<MeshRenderer>();

        // Use the instance material (Renderer.material) to modify the specific instance
        Material[] materials = sauceRenderer.materials;
        sauceRenderer.materials = materials;  // Assign the updated materials array
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Optionally, re-apply the material when the object is released, if needed
    }

    private void Update()
    {
        if (isPouring && isInsideCollider)
        {
            if (!doughParticles.isPlaying)
            {
                Debug.Log("Playing Particle");
                doughParticles.Play();
            }
            currentScalingTime += Time.deltaTime;

            float progress = Mathf.Clamp01(currentScalingTime / scalingTime);
            sauceAnchor.localScale = Vector3.Lerp(initialScale, targetScale, progress);

            if (currentScalingTime >= scalingTime || sauceAnchor.localScale == targetScale)
            {
                // Modify the instance material only for the currently grabbed object
                Material[] materials = sauceRenderer.materials;

                // Create a new array with two fewer elements
                Material[] newMaterials = new Material[materials.Length - 2];

                // Copy the first two elements from the original materials array
                newMaterials[0] = materials[0];
                newMaterials[1] = transparentMaterial;  // or materials[1] if you want to keep the original

                // Assign the new materials array back to the renderer
                sauceRenderer.materials = newMaterials;

                isPouring = false;
                doughParticles.Stop();

                if(nextItem != null)
                {
                    nextItem.SetActive(true);
                    nextItemOutline = nextItem.GetComponent<Outline>();
                    if (nextItemOutline != null) nextItemOutline.enabled = true;
                    nextItemOutline2.enabled = true;
                }
                
                sauceOutline.enabled = false;
                if (nextNarration != null)
                {
                    nextNarration.StartNarration();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SauceBowl"))
        {
            Debug.Log("Sauce entered collider");
            isInsideCollider = true;
            CheckForPouring(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SauceBowl"))
        {
            Debug.Log("Sauce exit collider");
            isInsideCollider = false;
            isPouring = false;
            if (doughParticles.isPlaying)
            {
                Debug.Log("Stopping Particle");
                doughParticles.Stop();
            }
        }
    }

    private void CheckForPouring(Transform sauceBowl)
    {
        if (isInsideCollider && sauceAnchor.localScale != targetScale)
        {
            float xTilt = sauceBowl.eulerAngles.x;
            float zTilt = sauceBowl.eulerAngles.z;

            xTilt = (xTilt > 180) ? xTilt - 360 : xTilt;
            zTilt = (zTilt > 180) ? zTilt - 360 : zTilt;

            if ((xTilt < -minTiltAngle || xTilt > maxTiltAngle) || (zTilt < -minTiltAngle || zTilt > maxTiltAngle))
            {
                isPouring = false;
                if (doughParticles.isPlaying)
                {
                    Debug.Log("Stopping Particle");
                    doughParticles.Stop();
                }
            }
            else
            {
                isPouring = true;
                if (!doughParticles.isPlaying)
                {
                    Debug.Log("Playing Particle");
                    doughParticles.Play();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SauceBowl"))
        {
            CheckForPouring(other.transform);
        }
    }
}

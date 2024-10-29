using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationDuration = 1.5f;
    private float rotationTime = 0f;
    private bool shouldRotate = false;
    private Quaternion startRotation;
    public Quaternion endRotation;

    void Update()
    {
        if (shouldRotate)
        {
            
            rotationTime += Time.deltaTime;
            float t = Mathf.Clamp01(rotationTime / rotationDuration);
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            //Debug.Log($"Turning... Dial Rotation: {transform.rotation.eulerAngles}");

            if (t >= 1f)
            {
                shouldRotate = false;
                Outline outline = gameObject.GetComponent<Outline>();
                outline.enabled = false;
            }
        }
    }

    public void StartRotation()
    {
        startRotation = transform.localRotation;
        rotationTime = 0f;
        shouldRotate = true;
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimpleSliderRaycaster : MonoBehaviour
{
    public Camera mainCamera;

    private void Start()
    {
        // Find and set the Main Camera
        GameObject mainCam = GameObject.Find("Main Camera");
        if (mainCam != null)
        {
            mainCamera = mainCam.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    void Update()
    {
        // Check if the left mouse button is pressed
        if (Mouse.current.leftButton.isPressed)
        {
            // Perform a raycast from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has a Slider component
                Slider slider = hit.collider.GetComponent<Slider>();
                if (slider != null)
                {
                    // Calculate the normalized value based on mouse position
                    RectTransform sliderRectTransform = slider.GetComponent<RectTransform>();
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRectTransform, Mouse.current.position.ReadValue(), mainCamera, out localPoint);

                    // Calculate normalized value
                    float normalizedValue = Mathf.Clamp01((localPoint.x - sliderRectTransform.rect.xMin) / sliderRectTransform.rect.width);
                    slider.value = normalizedValue; // Update slider value

                    Debug.Log("Slider Value: " + normalizedValue); // Log for debugging
                }
            }
        }
    }
}

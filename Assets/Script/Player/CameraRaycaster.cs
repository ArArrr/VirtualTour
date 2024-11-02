using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraRaycaster : MonoBehaviour
{
    public Camera mainCamera;
    public float raycastDistance = 100f;
    public LayerMask interactableLayerMask; // Optional: You can define which layers are interactable
    public MouseControl mouseControl;

    private void Start()
    {
        GameObject mainCam = GameObject.Find("Main Camera");
        mainCamera = mainCam.GetComponent<Camera>();
        mouseControl = mainCam.GetComponent<MouseControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // Perform a raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Check if the left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame && mouseControl.isActive)
        {
            // Check for UI elements
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Screen.width / 2, Screen.height / 2) // Center of the screen
            };

            // Raycast through UI elements
            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    // Check for Button and invoke click
                    var button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke();
                        return;
                    }

                    // Check for Dropdown and show/hide the dropdown menu
                    var dropdown = result.gameObject.GetComponent<TMP_Dropdown>();
                    if (dropdown != null)
                    {
                        dropdown.Show(); // Opens the dropdown menu
                        return;
                    }

                    // Check for InputField and activate the input
                    var inputField = result.gameObject.GetComponent<TMP_InputField>();
                    if (inputField != null)
                    {
                        inputField.ActivateInputField(); // Focuses on the input field
                        return;
                    }

                    // Check for Toggle and toggle its value
                    var toggle = result.gameObject.GetComponent<Toggle>();
                    if (toggle != null)
                    {
                        toggle.isOn = !toggle.isOn; // Toggles the current value
                        return;
                    }

                    // Check for Slider and set its value
                    var slider = result.gameObject.GetComponent<Slider>();
                    if (slider != null)
                    {
                        // Get the RectTransform component of the slider
                        RectTransform sliderRectTransform = slider.GetComponent<RectTransform>();

                        // Get the current mouse position using the new Input System
                        Vector2 mousePosition = Mouse.current.position.ReadValue();
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRectTransform, mousePosition, mainCamera, out Vector2 localPoint);

                        // Calculate the normalized value from the local point on the slider
                        float normalizedValue = Mathf.Clamp01((localPoint.x - sliderRectTransform.rect.xMin) / (sliderRectTransform.rect.width));
                        slider.value = normalizedValue; // Set the slider value
                        return;
                    }
                }
            }
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraRaycaster : MonoBehaviour
{
    public Camera mainCamera;
    public float raycastDistance = 100f;
    public LayerMask interactableLayerMask;
    public MouseControl mouseControl;

    private Slider activeSlider = null;

    private void Start()
    {
        GameObject mainCam = GameObject.Find("Main Camera");
        mainCamera = mainCam.GetComponent<Camera>();
        mouseControl = mainCam.GetComponent<MouseControl>();
    }

    void Update()
    {
        // Perform a raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Check if the left mouse button is pressed and mouse control is active
        if (Mouse.current.leftButton.isPressed && mouseControl.isActive)
        {
            // If we are interacting with a slider, continue updating its value
            if (activeSlider != null)
            {
                UpdateSliderValue(activeSlider);
                return;
            }

            // Check for UI elements on initial click
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
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
                            dropdown.Show();
                            return;
                        }

                        // Check for InputField and activate the input
                        var inputField = result.gameObject.GetComponent<TMP_InputField>();
                        if (inputField != null)
                        {
                            inputField.ActivateInputField();
                            return;
                        }

                        // Check for Toggle and toggle its value
                        var toggle = result.gameObject.GetComponent<Toggle>();
                        if (toggle != null)
                        {
                            toggle.isOn = !toggle.isOn;
                            return;
                        }

                        // Check for Slider and start holding interaction
                        var slider = result.gameObject.GetComponent<Slider>();
                        if (slider != null)
                        {
                            activeSlider = slider;
                            UpdateSliderValue(slider); // Initial update
                            return;
                        }
                    }
                }
            }
        }

        // Reset the active slider when the mouse button is released
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            activeSlider = null;
        }
    }

    // Update the slider value based on mouse position
    private void UpdateSliderValue(Slider slider)
    {
        RectTransform sliderRectTransform = slider.GetComponent<RectTransform>();

        // Get the current mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(sliderRectTransform, mousePosition, mainCamera, out Vector2 localPoint);

        // Calculate the normalized value from the local point on the slider
        float normalizedValue = Mathf.Clamp01((localPoint.x - sliderRectTransform.rect.xMin) / sliderRectTransform.rect.width);
        slider.value = normalizedValue;
    }
}

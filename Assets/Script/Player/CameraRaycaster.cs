using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraRaycaster : MonoBehaviour
{
    public Camera mainCamera;
    public float raycastDistance = 100f;
    public LayerMask interactableLayerMask; // Optional: You can define which layers are interactable
    private void Start()
    {
        GameObject mainCam = GameObject.Find("Main Camera");
        mainCamera = mainCam.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Perform a raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        //RaycastHit hit;

        // Check if the left mouse button is clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //// Debugging raycast
            //Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 1f);

            //// Check if the ray hits anything on the interactable layer
            //if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayerMask))
            //{
            //    Debug.Log("Hit interactable: " + hit.collider.gameObject.name);

            //    // Optionally trigger an interaction or click event
            //    if (hit.collider.gameObject.CompareTag("Interactable"))
            //    {
            //        // Call a method on the hit object
            //        hit.collider.gameObject.SendMessage("OnRaycastHit", SendMessageOptions.DontRequireReceiver);
            //    }
            //}
            //else
            //{
            //    Debug.Log("No interactable hit.");
            //}

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
                    //Debug.Log("UI Element Hit: " + result.gameObject.name);

                    // Trigger button click if the hit object is a button
                    var button = result.gameObject.GetComponent<UnityEngine.UI.Button>();
                    if (button != null)
                    {
                        button.onClick.Invoke(); // Only invoke the click on press
                        //Debug.Log("Button clicked: " + button.gameObject.name);
                    }
                }
            }
            else
            {
                //Debug.Log("No UI element hit.");
            }
        }
    }
}

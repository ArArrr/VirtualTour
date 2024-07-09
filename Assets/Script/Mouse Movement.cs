using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform headTransform;
    public Transform playerTransform;

    float xRotation = 0f;

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Control rotation around x axis (Look up and down)
        xRotation -= mouseY;

        // Clamp the rotation so we can't over-rotate (like in real life)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the head only in the x-axis
        headTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player object in the y-axis (left and right)
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}

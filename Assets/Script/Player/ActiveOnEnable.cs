using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.Interaction.Toolkit.UI
{
    public class ActiveOnEnable : MonoBehaviour
    {
        public GameObject CameraOffset;
        public List<GameObject> ToActivate;
        public List<GameObject> ToDeactivate;
        public GameObject XROrigin;
        public Vector3 cameraOffsetPosition = new Vector3(0, 1.55f, 0); // Set default value

        private void Start()
        {
            CameraOffset = GameObject.Find("Camera Offset");
            XROrigin = GameObject.Find("XR Origin (VR)");
            GameObject subtitle = GameObject.Find("Subtitle UI");
            if (subtitle != null)
            {
                SubtitleUIController uIController = subtitle.GetComponent<SubtitleUIController>();
                if (uIController != null && subtitle != null) uIController.smoothSpeed = 50;
            }
            OnEnable();
        }

        private void OnEnable()
        {
            if (CameraOffset != null)
            {
                CameraOffset.transform.localPosition = cameraOffsetPosition;
            }

            // Find the EventSystem object and disable mouse input
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem != null)
            {
                XRUIInputModule xruiInputModule = eventSystem.GetComponent<XRUIInputModule>();
                if (xruiInputModule != null)
                {
                    // Set m_EnableMouseInput to false
                    xruiInputModule.m_EnableMouseInput = false;
                }
            }

            ActivateGrabRay grabray;
            if (XROrigin != null)
            {
                grabray = XROrigin.GetComponent<ActivateGrabRay>();
                grabray.enabled = false;
            }

            // Find and deactivate the "Left Grab Ray" and "Right Grab Ray" objects
            GameObject leftGrabRay = GameObject.Find("Left Grab Ray");
            GameObject rightGrabRay = GameObject.Find("Right Grab Ray");
            if (leftGrabRay != null)
            {
                leftGrabRay.SetActive(false);
            }
            if (rightGrabRay != null)
            {
                rightGrabRay.SetActive(false);
            }

            // Find the Main Camera and enable its MouseControl component
            GameObject mainCamera = GameObject.Find("Main Camera");
            if (mainCamera != null)
            {
                MouseControl mouseControl = mainCamera.GetComponent<MouseControl>();
                if (mouseControl != null)
                {
                    mouseControl.enabled = true; // Enable the MouseControl component
                }
            }
            if (ToActivate != null)
            {
                foreach (GameObject gameObject in ToActivate)
                {
                    gameObject.SetActive(true);
                }
            }

            if(ToDeactivate != null)
            {
                foreach (GameObject gameObject in ToDeactivate)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnDisable()
        {
            if (CameraOffset != null)
            {
                CameraOffset.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}

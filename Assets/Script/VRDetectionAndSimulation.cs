using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRDetectionAndSimulation : MonoBehaviour
{
    public GameObject xrDeviceSimulatorPrefab; // Assign your XR Device Simulator prefab here

    void Start()
    {
        // Check if an XR device is connected
        if (XRSettings.isDeviceActive)
        {
            Debug.Log("VR Headset detected. Running in VR mode.");
            DeactivateXRDeviceSimulator();
        }
        else
        {
            Debug.Log("No VR Headset detected. Activating XR Device Simulator.");
            ActivateXRDeviceSimulator();
        }
    }

    void ActivateXRDeviceSimulator()
    {
        if (xrDeviceSimulatorPrefab != null)
        {
            xrDeviceSimulatorPrefab.SetActive(true);
        }
    }

    void DeactivateXRDeviceSimulator()
    {
        xrDeviceSimulatorPrefab.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnEnable : MonoBehaviour
{
    public GameObject CameraOffset;
    public List<GameObject> ToActivate;

    private void Start()
    {
        CameraOffset = GameObject.Find("Camera Offset");
        OnEnable();
    }

    private void OnEnable()
    {
        if (CameraOffset != null)
        {
            CameraOffset.transform.localPosition = new Vector3(0, 1.5f, 0);
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

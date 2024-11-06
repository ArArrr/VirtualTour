using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMode : MonoBehaviour
{
    public GameObject xrDevice;
    void Start()
    {
        if (DataManager.Instance.togglePC) xrDevice.SetActive(true);
    }

}

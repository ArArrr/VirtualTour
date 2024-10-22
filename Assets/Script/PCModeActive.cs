using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCModeActive : MonoBehaviour
{
    void Start()
    {
        if (!DataManager.Instance.togglePC)
        {
            gameObject.SetActive(false);
        }
    }
}

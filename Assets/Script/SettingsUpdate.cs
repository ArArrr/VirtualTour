using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUpdate : MonoBehaviour
{
    public Toggle togglePC;
    public TMP_Dropdown turnDropdown;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.togglePC)
        {
            togglePC.isOn = true;
        }
        if (DataManager.Instance.turnMethod.Equals("continious"))
        {
            turnDropdown.value = 0;
        }
        if (DataManager.Instance.turnMethod.Equals("snap"))
        {
            turnDropdown.value = 1;
        }
    }
}

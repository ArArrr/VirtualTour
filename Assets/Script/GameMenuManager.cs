using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float spawnDistance = 2;
    public GameObject menu;
    public bool trackYAxis = false;
    public InputActionProperty showButton;

    private void Start()
    {
        // Automatically get the Main Camera's Transform component
        head = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);
            if(menu.activeSelf) DataManager.Instance.isInMenu = true;
            else DataManager.Instance.isInMenu = false;
            menu.transform.position = head.position + new Vector3(head.forward.x, trackYAxis ? head.forward.y : 0, head.forward.z).normalized * spawnDistance;
        }

        menu.transform.LookAt(new Vector3(head.position.x, trackYAxis ? head.position.y : menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}

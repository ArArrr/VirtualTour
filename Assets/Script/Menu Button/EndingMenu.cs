using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Thankyou;

    private void Start()
    {
        if (DataManager.Instance.lastCompletedFloor == 13)
        {
            MainMenu.SetActive(false);
            Thankyou.SetActive(true);
        }
    }
}

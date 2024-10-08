using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    GameObject block;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.isTour)
        {
            block.SetActive(true);
        }
        else
        {
            block.SetActive(false);
        }
    }


}

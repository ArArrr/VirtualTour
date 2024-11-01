using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    public ActionBasedContinuousTurnProvider continousTurn;
    public ActionBasedSnapTurnProvider snapTurn;
    public TMP_Dropdown Dropdown;

    private void Start()
    {
        if (DataManager.Instance.togglePC)
        {
            Dropdown.interactable = false;
            return;
        }
        int playerTurnSetting = DataManager.Instance.turnMethod.ToLower().Equals("continious") ? 0 : 1;
        SetTypeFromIndem(playerTurnSetting);
    }

    public void SetTypeFromIndem(int index)
    {
        if(index == 0)
        {
            snapTurn.enabled = false;
            continousTurn.enabled = true;
            DataManager.Instance.turnMethod = "continious";
        }
        else if (index == 1) 
        {
            snapTurn.enabled = true;
            continousTurn.enabled = false;
            DataManager.Instance.turnMethod = "snap";
        }
    }
}

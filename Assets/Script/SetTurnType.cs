using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    public ActionBasedContinuousTurnProvider continousTurn;
    public ActionBasedSnapTurnProvider snapTurn;

    private void Start()
    {
        int playerTurnSetting = DataManager.Instance.turnMethod.Equals("continious") ? 0 : 1;
        SetTypeFromIndem(playerTurnSetting);
    }

    public void SetTypeFromIndem(int index)
    {
        if(index == 0)
        {
            snapTurn.enabled = false;
            continousTurn.enabled = true;
        }
        else if (index == 1) 
        {
            snapTurn.enabled = true;
            continousTurn.enabled = false;
        }
    }
}

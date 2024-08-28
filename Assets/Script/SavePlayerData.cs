using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SavePlayerData : MonoBehaviour
{
    public TMP_InputField m_playerName;
    public string m_gender;
    public string m_yearLevel;
    public string m_strand;
    public string m_movement;
    public string m_turn;
    public void setPlayerName()
    {
        DataManager.Instance.playerName = m_playerName.text;
        Debug.Log("Set Player Name to " + DataManager.Instance.playerName);
    }
    public void setGender()
    {
        DataManager.Instance.gender = m_gender;
        Debug.Log("Set Gender to " + DataManager.Instance.gender);
    }
    public void setYearLevel()
    {
        DataManager.Instance.yearLevel = m_yearLevel;
    }
    public void setStrand()
    {
        DataManager.Instance.strand = m_strand;
    }
    public void setMovement()
    {
        DataManager.Instance.moveMethod = m_movement;
    }
    public void setTurn()
    {
        DataManager.Instance.turnMethod = m_turn;
    }
    public void SaveToFile()
    {
        string playerName = m_playerName.IsUnityNull() ? DataManager.Instance.playerName : m_playerName.text;
        string gender = m_gender.IsUnityNull() ? DataManager.Instance.gender : m_gender;
        string yearLevel = m_yearLevel.IsUnityNull() ? DataManager.Instance.yearLevel : m_yearLevel;
        string strand = m_strand.IsUnityNull() ? DataManager.Instance.strand : m_strand;
        string movement = m_movement.IsUnityNull() ? DataManager.Instance.moveMethod : m_movement;
        string turn = m_turn.IsUnityNull() ? DataManager.Instance.turnMethod : m_turn;

        DataManager.Instance.SavePlayerData(playerName, yearLevel, strand, gender, movement, turn);
    }
}

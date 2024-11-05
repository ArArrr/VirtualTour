using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Windows;

public class CharacterList : MonoBehaviour
{
    GameObject ID;
    GameObject Arrow;
    [System.Serializable]
    public class character
    {
        public GameObject characterObj;
        public string course;
        public enum genderList
        {
            Male,
            Female
        }
        public genderList Gender;

        public void activeChar()
        {
            if(DataManager.Instance.strand.ToLower() == course.ToLower() && Gender.ToString().Equals(DataManager.Instance.gender, StringComparison.OrdinalIgnoreCase))
            characterObj.SetActive(true);
        }
    }

    public List<character> characterList;

    private void Start()
    {
        foreach (var chars in characterList)
        {
            chars.activeChar();
        }
    }

    public void toggleArrow(bool b)
    {
        // Find all objects with the tag "IDCard"
        GameObject[] idCards = GameObject.FindGameObjectsWithTag("IDCard");

        // Loop through each IDCard object and check if it is active
        foreach (GameObject idCard in idCards)
        {
            if (idCard.activeSelf)
            {
                // Set the active IDCard to ID
                ID = idCard;

                // Find the child object named "Arrow" within the active IDCard
                Arrow = ID.transform.Find("Arrow")?.gameObject;

                // Check if Arrow was found and set it active or inactive based on 'b'
                if (Arrow != null)
                {
                    Arrow.SetActive(b);
                }
                else
                {
                    Debug.LogWarning("Arrow child not found under the active IDCard.");
                }

                // Since only one IDCard is active at a time, we can exit the loop
                break;
            }
        }
    }

}

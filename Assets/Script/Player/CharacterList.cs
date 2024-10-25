using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Windows;

public class CharacterList : MonoBehaviour
{
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
}

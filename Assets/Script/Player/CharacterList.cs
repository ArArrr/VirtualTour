using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

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
    }

    public List<character> characterList;
}

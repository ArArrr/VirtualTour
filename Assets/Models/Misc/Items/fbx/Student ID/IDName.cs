using UnityEngine;
using TMPro;

public class IDName : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        // Format the name to ensure the first letter is capitalized
        string formattedName = DataManager.Instance.playerName;

        if (!string.IsNullOrEmpty(formattedName))
        {
            formattedName = char.ToUpper(formattedName[0]) + formattedName.Substring(1).ToLower();
        }

        text.text = formattedName;
    }
}

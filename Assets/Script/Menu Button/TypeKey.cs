using UnityEngine;
using TMPro;  // Ensure you include this for TextMeshPro

public class TypeKey : MonoBehaviour
{
    public char key;  // The character that this button represents
    public TMP_InputField inputField;  // Reference to the InputField

    // This method will be called when the button is pressed
    public void OnKeyPressed()
    {
        if (inputField.text.Length < 14)
        inputField.text += key;
    }
    public void OnBackspacePressed()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }
}

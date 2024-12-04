using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberInputField : MonoBehaviour
{
    [Tooltip("Reference to the TMP InputField component.")]
    public TMP_InputField inputField;
    public Toggle toggle;

    [Tooltip("Default fallback value if input is invalid.")]
    public float defaultValue = 5f;
    private float timer = 0f;
    

    private void Start()
    {
        // Ensure only numbers are allowed in the input field
        inputField.onValueChanged.AddListener(ValidateInput);
        inputField.onEndEdit.AddListener(HandleEndEdit);
        toggle.isOn = DataManager.Instance.isDemo;
        inputField.text = DataManager.Instance.timer.ToString();
    }

    // Validate input to allow only numbers
    private void ValidateInput(string input)
    {
        // Remove non-numeric characters except for a single dot
        string validInput = "";
        bool dotUsed = false;

        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                validInput += c;
            }
            else if (c == '.' && !dotUsed)
            {
                validInput += c;
                dotUsed = true;
            }
        }

        if (input != validInput)
        {
            inputField.text = validInput;
        }
    }

    // Handle the end of editing: convert input to float or fallback to default
    private void HandleEndEdit(string input)
    {
        if (!float.TryParse(input, out float result))
        {
            inputField.text = defaultValue.ToString();
        }
    }

    public void SetTimer()
    {
        
        string num = inputField.text;
        // Try to parse the string to a float
        if (float.TryParse(num, out float result))
        {
            timer = result;
        }
        else
        {
            Debug.LogWarning($"Invalid input '{num}'. Setting timer to default value {defaultValue}.");
            timer = defaultValue;
        }
        DataManager.Instance.timer = timer;
    }

    public void setDemoMode(bool demoMode)
    {
        DataManager.Instance.isDemo = demoMode;
    }
}

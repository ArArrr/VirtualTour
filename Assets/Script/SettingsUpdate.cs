using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUpdate : MonoBehaviour
{
    public Toggle togglePC;
    public TMP_Dropdown turnDropdown;

    void Start()
    {
        // Initialize the toggle and dropdown based on saved values
        togglePC.isOn = DataManager.Instance.togglePC;

        // Set dropdown based on turn method in DataManager
        turnDropdown.value = DataManager.Instance.turnMethod.Equals("continuous") ? 0 : 1;

        // Add listener for toggle changes
        togglePC.onValueChanged.AddListener(OnTogglePCChanged);
    }

    // This method is called whenever the toggle's value changes
    private void OnTogglePCChanged(bool isOn)
    {
        DataManager.Instance.togglePC = isOn;
    }

    void OnDestroy()
    {
        // Remove listener when the object is destroyed to prevent memory leaks
        togglePC.onValueChanged.RemoveListener(OnTogglePCChanged);
    }

    public void exitGame()
    {
        // Log for when the game is closed (only visible in Editor)
        Debug.Log("Quitting the game...");

        // Closes the application
        Application.Quit();

#if UNITY_EDITOR
        // Stops playing in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

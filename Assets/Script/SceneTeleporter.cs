using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderUI : MonoBehaviour
{
    public TMPro.TMP_Dropdown sceneDropdown; // Reference to the dropdown UI element
    public Button loadSceneButton; // Reference to the button UI element
    public string transition = "CrossFade";

    private void Start()
    {
        // Add listener for the button click
        loadSceneButton.onClick.AddListener(OnLoadSceneButtonClicked);
    }

    private void OnLoadSceneButtonClicked()
    {
        // Get the selected scene name from the dropdown
        string selectedScene = sceneDropdown.options[sceneDropdown.value].text;

        // Load the selected scene
        LevelManager.Instance.LoadScene(selectedScene, transition, "none");
    }


}

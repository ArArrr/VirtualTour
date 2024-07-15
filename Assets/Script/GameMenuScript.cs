using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuScript : MonoBehaviour
{

    public Button loadSceneButton; // Reference to the button UI element

    private void Start()
    {
        // Add listener for the button click
        loadSceneButton.onClick.AddListener(OnLoadSceneButtonClicked);
    }

    private void OnLoadSceneButtonClicked()
    {
        // Get the selected scene name from the dropdown
        string selectedScene = "2FR Hallway";

        // Load the selected scene
        SceneManager.LoadScene(selectedScene);
    }
}

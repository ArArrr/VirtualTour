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
        if (selectedScene == "Complete" || selectedScene == "Career Camps") selectedScene = "1F Lobby";

        DataManager.Instance.lastCompletedFloor = getLevel();
        // Load the selected scene
        LevelManager.Instance.LoadScene(selectedScene, transition, "none");
    }

    public int getLevel()
    {
        DataManager.Instance.targetSpawnPointID = "default";
        switch(sceneDropdown.options[sceneDropdown.value].text)
        {
            case "Main Menu": return 0; 
            case "Outdoor": return 0;
            case "1F Lobby": DataManager.Instance.isTour = true; return 0;
            case "BF Canteen": DataManager.Instance.isTour = true; return 1;
            case "8F Court": DataManager.Instance.isTour = true; return 2;
            case "7F Hallway": DataManager.Instance.isTour = true; return 3;
            case "6F Hallway": DataManager.Instance.isTour = true; return 4;
            case "5F Hallway": DataManager.Instance.isTour = true; return 5;
            case "4F Hallwayn": DataManager.Instance.isTour = true; return 6;
            case "3F Hallway": DataManager.Instance.isTour = true; return 7;
            case "2F Hallway": DataManager.Instance.isTour = true; return 8;
            case "Complete": DataManager.Instance.targetSpawnPointID = "Lobby"; DataManager.Instance.isTour = false; return 9; 
            case "Demo Scene": return 0;
            case "Career Camps": DataManager.Instance.targetSpawnPointID = "Lobby"; DataManager.Instance.isTour = false; return 14;
            default: return 0;
        }
    }
}

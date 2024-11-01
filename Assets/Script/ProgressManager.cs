using System.IO;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private PlayerProgress currentProgress;

    void Start()
    {
        // Load the player's progress when the game starts
        LoadGameProgress();
    }

    public void CompleteFloor(int completedFloor)
    {
        // Update the player's progress
        //currentProgress.lastCompletedFloor = completedFloor;

        // Save the progress
        SaveGameProgress();
    }

    public void saveInfo()
    {
        currentProgress.playerName = DataManager.Instance.playerName;
        currentProgress.gender = DataManager.Instance.gender;
        currentProgress.yearLevel = DataManager.Instance.yearLevel;
        currentProgress.strand = DataManager.Instance.strand;
        currentProgress.moveMethod = DataManager.Instance.moveMethod;
        currentProgress.turnMethod = DataManager.Instance.turnMethod;
        SaveGameProgress();
    }

    public void SaveGameProgress()
    {
        SaveSystem.SaveProgress(currentProgress);
        Debug.Log("Game progress saved: " + JsonUtility.ToJson(currentProgress));
    }

    public void LoadGameProgress()
    {
        currentProgress = SaveSystem.LoadProgress();
        if (currentProgress != null)
        {
            // Load data into PersistentDataManager if necessary
            //DataManager.Instance.lastCompletedFloor = currentProgress.lastCompletedFloor;
            DataManager.Instance.playerName = currentProgress.playerName;
            DataManager.Instance.gender = currentProgress.gender;
            DataManager.Instance.yearLevel = currentProgress.yearLevel;
            DataManager.Instance.strand = currentProgress.strand;
            DataManager.Instance.moveMethod = currentProgress.moveMethod;
            DataManager.Instance.turnMethod = currentProgress.turnMethod;
            DataManager.Instance.masterVolume = currentProgress.masterVolume;
            DataManager.Instance.mouseSensitivity = currentProgress.mouseSensitivity;
            DataManager.Instance.musicVolume = currentProgress.musicVolume;

            Debug.Log("Game progress loaded: " + JsonUtility.ToJson(currentProgress));
        }
        else
        {
            Debug.Log("No previous save found.");
            currentProgress = new PlayerProgress(); // Create a new instance if no save exists
        }
    }
    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted.");
        }
    }
}


[System.Serializable]
public class PlayerProgress
{
    //public int lastCompletedFloor;
    public string playerName;
    public string gender;
    public string yearLevel;
    public string strand;
    

    public float masterVolume = 1f;
    public float musicVolume = 0.5f;
    public string moveMethod;
    public string turnMethod;
    public float mouseSensitivity = 60;
}

public class SaveSystem
{
    public static void SaveProgress(PlayerProgress progress)
    {
        string json = JsonUtility.ToJson(progress);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static PlayerProgress LoadProgress()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<PlayerProgress>(json);
        }
        return null; // No save file found
    }
}

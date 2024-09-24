using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Step 2: Create a static instance for the Singleton
    public static DataManager Instance { get; private set; }

    // Step 3: Add fields for your player data
    public int lastCompletedFloor;
    public string playerName;
    public string yearLevel;
    public string strand;
    public string gender;
    public string moveMethod;
    public string turnMethod;

    // SETTINGS
    public bool togglePC = false;

    public string targetSpawnPointID;

    void Awake()
    {
        // Step 4: Implement Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This will make this object persist between scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate managers
        }
    }

    // Optionally, create methods to load and save player data
    public void SavePlayerData(string name, string year, string strand, string gender, string move, string turn)
    {
        playerName = name;
        yearLevel = year;
        this.strand = strand;
        this.gender = gender;
        moveMethod = move;
        turnMethod = turn;
    }

    public void LoadPlayerData()
    {
        // Example method to load data (you can implement JSON/PlayerPrefs here)
        Debug.Log($"Loaded: {playerName}, {yearLevel}, {strand}, {gender}, {moveMethod}, {turnMethod}");
    }

    public void togglePCSetting(bool value)
    {
        togglePC = value;
    }
}

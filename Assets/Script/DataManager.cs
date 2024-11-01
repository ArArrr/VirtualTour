using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Step 2: Create a static instance for the Singleton
    public static DataManager Instance { get; private set; }

    public bool cameraInUse = false;

    // Step 3: Add fields for your player data
    [Header("Player Data")]
    public int lastCompletedFloor = 0;
    public string playerName;
    public string yearLevel;
    public string strand;
    public string gender;
    public string targetSpawnPointID;
    public bool isTour = false;
    public bool nextLevel = false;
    public string camp;
    public bool isHoldingItem = false;

    // SETTINGS
    [Header("Settings")]
    public float masterVolume = 1.0f;
    public float musicVolume = 0.5f;
    public bool isInMenu = false;
    public bool togglePC = false;
    public string moveMethod;
    public string turnMethod;

    [Header("Camp Career Data (Photography)")]
    public bool introDone = false;
    public bool firstShot = false;
    public int picCount = 1;

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
        lastCompletedFloor = 0;
    }

    public void LoadPlayerData()
    {
        // Example method to load data (you can implement JSON/PlayerPrefs here)
        Debug.Log($"Loaded: {playerName}, {yearLevel}, {strand}, {gender}, {moveMethod}, {turnMethod}");
    }

    public void togglePCSetting()
    {
        togglePC = !togglePC;
    }
}

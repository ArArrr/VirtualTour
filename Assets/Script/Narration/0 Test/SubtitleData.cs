using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleData", menuName = "Subtitles/SubtitleData")]
public class SubtitleData : ScriptableObject
{
    public AudioClip audioClip;
    public SubtitleLine[] subtitleLines;
}

[System.Serializable]
public class SubtitleLine
{
    public string text;
    public float startTime; // When the subtitle starts
    public float endTime;   // When the subtitle ends
}

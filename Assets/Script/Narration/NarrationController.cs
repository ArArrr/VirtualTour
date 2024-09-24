using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NarrationController : MonoBehaviour
{
    public SubtitleData subtitleData;  // Assign your subtitle data in the inspector
    public Text subtitleText;          // UI Text component for subtitles
    public AudioSource audioSource;    // AudioSource for playing narration audio

    private void Start()
    {
        StartCoroutine(PlayNarrationWithSubtitles());
    }

    private IEnumerator PlayNarrationWithSubtitles()
    {
        // Play the audio
        audioSource.clip = subtitleData.audioClip;
        audioSource.Play();

        foreach (var line in subtitleData.subtitleLines)
        {
            // Wait until it's time to show the next subtitle
            yield return new WaitForSeconds(line.startTime);

            // Show the subtitle
            subtitleText.text = line.text;

            // Wait until the subtitle ends, then clear it
            yield return new WaitForSeconds(line.endTime - line.startTime);
            subtitleText.text = "";
        }

        // Optionally, wait until the entire audio clip is finished
        yield return new WaitForSeconds(audioSource.clip.length);
    }
}

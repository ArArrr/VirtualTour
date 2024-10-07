using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSpawn : MonoBehaviour
{
    [SerializeField] ElevatorButtonController elevatorController;
    [SerializeField] ElevatorAudioController elevatorAudio;
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance.targetSpawnPointID == elevatorAudio.spawnID)
        StartCoroutine(enumerator());
    }
    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(elevatorController.HandleSpawnElevator());
    }
}

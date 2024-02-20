using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMusicDoor : MonoBehaviour
{
    Doorway doorScript;
    MusicManager musicManager;

    private void Awake()
    {
        doorScript = GetComponent<Doorway>();
        musicManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MusicManager>();
    }

    private void OnEnable()
    {
        doorScript.OnOpenDoor += DoorScript_OnOpenDoor;
    }

    private void OnDisable()
    {
        doorScript.OnOpenDoor -= DoorScript_OnOpenDoor;
    }

    private void DoorScript_OnOpenDoor(object sender, System.EventArgs e)
    {
        musicManager.StopImmediate();
    }
}

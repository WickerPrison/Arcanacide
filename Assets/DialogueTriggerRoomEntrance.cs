using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerRoomEntrance : MonoBehaviour
{
    Dialogue dialogue;

    private void Start()
    {
        dialogue = GetComponent<Dialogue>();
        dialogue.StartConversation();
    }
}

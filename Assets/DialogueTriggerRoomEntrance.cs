using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class DialogueTriggerRoomEntrance : MonoBehaviour
{
    Dialogue dialogue;
    bool hasHadConversation = false;

    private void Start()
    {
        dialogue = GetComponent<Dialogue>();
        if (!hasHadConversation)
        {
            hasHadConversation = true;
            dialogue.StartConversation();
        }
    }
}

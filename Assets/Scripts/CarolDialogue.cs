using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarolDialogue : MonoBehaviour
{
    [SerializeField] MapData mapData;
    Dialogue dialogue;

    private void Awake()
    {
        dialogue = GetComponent<Dialogue>();
        if(mapData.carolsDeadFriends.Count == 0)
        {
            dialogue.conversationNum = 0;
        }
        else if(mapData.carolsDeadFriends.Count < 3)
        {
            dialogue.conversationNum = 1;
        }
        else
        {
            dialogue.conversationNum = 2;
        }
    }
}

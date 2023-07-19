using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] string contactName;
    [SerializeField] int conversationNum;
    [SerializeField] DialogueData phoneData;

    private void Start()
    {
        switch (contactName)
        {
            case "ORTHODOX":
                if(!phoneData.directorQueue.Contains(conversationNum) && !phoneData.directorPreviousConversations.Contains(conversationNum))
                {
                    phoneData.directorQueue.Add(conversationNum);
                }
                break;
            case "??????":
                if (!phoneData.UnknownNumberQueue.Contains(conversationNum) && !phoneData.UnknownNumberPreviousConversations.Contains(conversationNum))
                {
                    phoneData.UnknownNumberQueue.Add(conversationNum);
                }
                break;
        }
    }

}

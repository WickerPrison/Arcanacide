using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] string contactName;
    [SerializeField] int conversationNum;
    [SerializeField] DialogueData dialogueData;

    private void Start()
    {
        switch (contactName)
        {
            case "DirectorWilkins":
                if(!dialogueData.directorQueue.Contains(conversationNum) && !dialogueData.directorPreviousConversations.Contains(conversationNum))
                {
                    dialogueData.directorQueue.Add(conversationNum);
                }
                break;
            case "AgentFrei":
                if(!dialogueData.freiQueue.Contains(conversationNum) && !dialogueData.freiPreviousConversations.Contains(conversationNum))
                {
                    dialogueData.freiQueue.Add(conversationNum);
                }
                break;
            case "??????":
                if (!dialogueData.UnknownNumberQueue.Contains(conversationNum) && !dialogueData.UnknownNumberPreviousConversations.Contains(conversationNum))
                {
                    dialogueData.UnknownNumberQueue.Add(conversationNum);
                }
                break;
            default:
                Debug.Log(contactName + "is not a valid contact name");
                break;
        }
    }

}

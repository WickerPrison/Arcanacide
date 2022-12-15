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
                if(!phoneData.ORTHODOXQueue.Contains(conversationNum) && !phoneData.ORTHODOXPreviousConversations.Contains(conversationNum))
                {
                    phoneData.ORTHODOXQueue.Add(conversationNum);
                }
                break;
            case "TRENCH":
                if (!phoneData.TRENCHQueue.Contains(conversationNum) && !phoneData.TRENCHPreviousConversations.Contains(conversationNum))
                {
                    phoneData.TRENCHQueue.Add(conversationNum);
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

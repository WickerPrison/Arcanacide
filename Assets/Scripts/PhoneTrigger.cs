using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] string contactName;
    [SerializeField] int conversationNum;
    [SerializeField] PhoneData phoneData;

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
            case "Head of IT":
                if(!phoneData.HeadOfITQueue.Contains(conversationNum) && !phoneData.HeadOfITPreviousConversations.Contains(conversationNum))
                {
                    phoneData.HeadOfITQueue.Add(conversationNum);
                }
                break;
        }
    }

}

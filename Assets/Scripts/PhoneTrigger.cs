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
            case "ODPARCHMENT":
                if(!phoneData.ODPARCHMENTQueue.Contains(conversationNum) && !phoneData.ODPARCHMENTPreviousConversations.Contains(conversationNum))
                {
                    phoneData.ODPARCHMENTQueue.Add(conversationNum);
                }
                break;
            case "ODTRENCH":
                if (!phoneData.ODTRENCHQueue.Contains(conversationNum) && !phoneData.ODTRENCHPreviousConversations.Contains(conversationNum))
                {
                    phoneData.ODTRENCHQueue.Add(conversationNum);
                }
                break;
            case "??????":
                if (!phoneData.QuestionMarksQueue.Contains(conversationNum) && !phoneData.QuestionMarksPreviousConversations.Contains(conversationNum))
                {
                    phoneData.QuestionMarksQueue.Add(conversationNum);
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

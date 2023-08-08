using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneTrigger : MonoBehaviour
{
    [SerializeField] PhoneContacts contactName;
    [SerializeField] int conversationNum;
    [SerializeField] DialogueData dialogueData;

    public virtual void Start()
    {
        SetTexts();
    }

    public void SetTexts()
    {
        if (!dialogueData.GetQueue(contactName).Contains(conversationNum) && !dialogueData.GetPreviousConversations(contactName).Contains(conversationNum))
        {
            dialogueData.GetQueue(contactName).Add(conversationNum);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PhoneData : ScriptableObject
{
    string ODPARCHMENT = "ODPARCHMENT";
    public List<int> ODPARCHMENTQueue;
    public List<int> ODPARCHMENTPreviousConversations;
    string ODTRENCH = "ODTRENCH";
    public List<int> ODTRENCHQueue;
    public List<int> ODTRENCHPreviousConversations;
    string UnkownNumber = "Unknown Number";
    public List<int> UnknownNumberQueue;
    public List<int> UnknownNumberPreviousConversations;
    string HeadOfIT = "Head of IT";
    public List<int> HeadOfITQueue;
    public List<int> HeadOfITPreviousConversations;

    public List<string> GetNewMessages()
    {
        List<string> newMessages = new List<string>();

        if(HeadOfITQueue.Count > 0)
        {
            newMessages.Add(HeadOfIT);
        }

        if(ODPARCHMENTQueue.Count > 0)
        {
            newMessages.Add(ODPARCHMENT);
        }

        if(ODTRENCHQueue.Count > 0)
        {
            newMessages.Add(ODTRENCH);
        }

        if(UnknownNumberQueue.Count > 0)
        {
            newMessages.Add(UnkownNumber);
        }

        return newMessages;
    }

    public void GetContacts(out List<string> contacts, out List<string> newMessages)
    {
        contacts = new List<string>();
        newMessages = new List<string>();

        if (HeadOfITQueue.Count + HeadOfITPreviousConversations.Count > 0)
        {
            contacts.Add(HeadOfIT);
            if (HeadOfITQueue.Count > 0)
            {
                newMessages.Add(HeadOfIT);
            }
        }

        if(ODPARCHMENTQueue.Count + ODPARCHMENTPreviousConversations.Count > 0)
        {
            contacts.Add(ODPARCHMENT);
            if(ODPARCHMENTQueue.Count > 0)
            {
                newMessages.Add(ODPARCHMENT);
            }
        }

        if(ODTRENCHQueue.Count + ODTRENCHPreviousConversations.Count > 0)
        {
            contacts.Add(ODTRENCH);
            if(ODTRENCHQueue.Count > 0)
            {
                newMessages.Add(ODTRENCH);
            }
        }

        if(UnknownNumberQueue.Count + UnknownNumberPreviousConversations.Count > 0)
        {
            contacts.Add(UnkownNumber);
            if(UnknownNumberQueue.Count > 0)
            {
                newMessages.Add(UnkownNumber);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PhoneData : ScriptableObject
{
    string ORTHODOX = "ORTHODOX";
    public List<int> ORTHODOXQueue;
    public List<int> ORTHODOXPreviousConversations;
    string TRENCH = "TRENCH";
    public List<int> TRENCHQueue;
    public List<int> TRENCHPreviousConversations;
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

        if(ORTHODOXQueue.Count > 0)
        {
            newMessages.Add(ORTHODOX);
        }

        if(TRENCHQueue.Count > 0)
        {
            newMessages.Add(TRENCH);
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

        if(ORTHODOXQueue.Count + ORTHODOXPreviousConversations.Count > 0)
        {
            contacts.Add(ORTHODOX);
            if(ORTHODOXQueue.Count > 0)
            {
                newMessages.Add(ORTHODOX);
            }
        }

        if(TRENCHQueue.Count + TRENCHPreviousConversations.Count > 0)
        {
            contacts.Add(TRENCH);
            if(TRENCHQueue.Count > 0)
            {
                newMessages.Add(TRENCH);
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

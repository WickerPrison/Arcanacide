using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    public List<string> conversationsHad;

    string directorWilkins = "Director Wilkins";
    public List<int> directorQueue;
    public List<int> directorPreviousConversations;
    string UnkownNumber = "Unknown Number";
    public List<int> UnknownNumberQueue;
    public List<int> UnknownNumberPreviousConversations;

    public List<int> patchworkGaryConversations;
    public List<int> whistleBlowerConversations;

    public List<string> GetNewMessages()
    {
        List<string> newMessages = new List<string>();

        if(directorQueue.Count > 0)
        {
            newMessages.Add(directorWilkins);
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

        if(directorQueue.Count + directorPreviousConversations.Count > 0)
        {
            contacts.Add(directorWilkins);
            if(directorQueue.Count > 0)
            {
                newMessages.Add(directorWilkins);
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

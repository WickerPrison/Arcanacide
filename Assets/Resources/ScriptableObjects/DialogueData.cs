using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum PhoneContacts
{
    DIRECTORWILKINS, AGENTFREI, BONSAI, UNKNOWNNUMBER
}

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    public List<string> conversationsHad;

    string directorWilkins = "Director Wilkins";
    public List<int> directorQueue;
    public List<int> directorPreviousConversations;
    string agentFrei = "Agent Frei";
    public List<int> freiQueue;
    public List<int> freiPreviousConversations;
    string bonsai = "Bonsai";
    public List<int> bonsaiQueue;
    public List<int> bonsaiPreviousConversations;
    string unkownNumber = "Unknown Number";
    public List<int> unknownNumberQueue;
    public List<int> unknownNumberPreviousConversations;

    public List<int> patchworkGaryConversations;
    public List<int> whistleBlowerConversations;
    public List<int> freiConversations;


    public List<int> GetQueue(PhoneContacts name)
    {
        switch (name)
        {
            case PhoneContacts.DIRECTORWILKINS:
                return directorQueue;
            case PhoneContacts.AGENTFREI:
                return freiQueue;
            case PhoneContacts.BONSAI:
                return bonsaiQueue;
            case PhoneContacts.UNKNOWNNUMBER:
                return unknownNumberQueue;
            default:
                return null;
        }
    }

    public List<int> GetPreviousConversations(PhoneContacts name)
    {
        switch (name)
        {
            case PhoneContacts.DIRECTORWILKINS:
                return directorPreviousConversations;
            case PhoneContacts.AGENTFREI:
                return freiPreviousConversations;
            case PhoneContacts.BONSAI:
                return bonsaiPreviousConversations;
            case PhoneContacts.UNKNOWNNUMBER:
                return unknownNumberPreviousConversations;
            default:
                return null;
        }
    }

    public string GetContactString(PhoneContacts name)
    {
        switch (name)
        {
            case PhoneContacts.DIRECTORWILKINS:
                return directorWilkins;
            case PhoneContacts.AGENTFREI:
                return agentFrei;
            case PhoneContacts.BONSAI:
                return bonsai;
            case PhoneContacts.UNKNOWNNUMBER:
                return unkownNumber;
            default:
                return null;
        }
    }

    public List<string> GetNewMessages()
    {
        List<string> newMessages = new List<string>();

        if(directorQueue.Count > 0)
        {
            newMessages.Add(directorWilkins);
        }

        if(freiQueue.Count > 0)
        {
            newMessages.Add(agentFrei);
        }

        if(bonsaiQueue.Count > 0)
        {
            newMessages.Add(bonsai);
        }

        if(unknownNumberQueue.Count > 0)
        {
            newMessages.Add(unkownNumber);
        }

        return newMessages;
    }

    public void GetContacts(out List<PhoneContacts> contacts, out List<PhoneContacts> newMessages)
    {
        contacts = new List<PhoneContacts>();
        newMessages = new List<PhoneContacts>();

        if(directorQueue.Count + directorPreviousConversations.Count > 0)
        {
            contacts.Add(PhoneContacts.DIRECTORWILKINS);
            if(directorQueue.Count > 0)
            {
                newMessages.Add(PhoneContacts.DIRECTORWILKINS);
            }
        }

        if(freiQueue.Count + freiPreviousConversations.Count > 0)
        {
            contacts.Add(PhoneContacts.AGENTFREI);
            if(freiQueue.Count > 0)
            {
                newMessages.Add(PhoneContacts.AGENTFREI);
            }
        }

        if(bonsaiQueue.Count + bonsaiPreviousConversations.Count > 0)
        {
            contacts.Add(PhoneContacts.BONSAI);
            if(bonsaiQueue.Count > 0)
            {
                newMessages.Add(PhoneContacts.BONSAI);
            }
        }

        if(unknownNumberQueue.Count + unknownNumberPreviousConversations.Count > 0)
        {
            contacts.Add(PhoneContacts.UNKNOWNNUMBER);
            if(unknownNumberQueue.Count > 0)
            {
                newMessages.Add(PhoneContacts.UNKNOWNNUMBER);
            }
        }
    }
}

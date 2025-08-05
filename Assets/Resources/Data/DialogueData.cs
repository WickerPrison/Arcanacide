using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum PhoneContacts
{
    DIRECTORWILKINS, AGENTFREI, SMACKGPT, UNKNOWNNUMBER
}

[CreateAssetMenu]
public class DialogueData : ScriptableObject
{
    public List<string> conversationsHad;
    [System.NonSerialized] public string outsideFranksRoom = "Outside Frank's Bossroom";

    string directorWilkins = "Director Wilkins";
    public List<int> directorQueue;
    public List<int> directorPreviousConversations;
    string agentFrei = "Agent Frei";
    public List<int> freiQueue;
    public List<int> freiPreviousConversations;
    string smackGPT = "SmackGPT";
    public List<int> smackGPTQueue;
    public List<int> smackGPTPreviousConversations;
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
            case PhoneContacts.SMACKGPT:
                return smackGPTQueue;
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
            case PhoneContacts.SMACKGPT:
                return smackGPTPreviousConversations;
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
            case PhoneContacts.SMACKGPT:
                return smackGPT;
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

        if(smackGPTQueue.Count > 0)
        {
            newMessages.Add(smackGPT);
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

        if(smackGPTQueue.Count + smackGPTPreviousConversations.Count > 0)
        {
            contacts.Add(PhoneContacts.SMACKGPT);
            if(smackGPTQueue.Count > 0)
            {
                newMessages.Add(PhoneContacts.SMACKGPT);
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

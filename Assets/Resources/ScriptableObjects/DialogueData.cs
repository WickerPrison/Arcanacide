using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    string UnkownNumber = "Unknown Number";
    public List<int> UnknownNumberQueue;
    public List<int> UnknownNumberPreviousConversations;

    public List<int> patchworkGaryConversations;
    public List<int> whistleBlowerConversations;


    public List<int> GetQueue(string name)
    {
        switch (name)
        {
            case "Director Wilkins":
                return directorQueue;
            case "Agent Frei":
                return freiQueue;
            case "Bonsai":
                return bonsaiQueue;
            default:
                return null;
        }
    }

    public List<int> GetPreviousConversations(string name)
    {
        switch (name)
        {
            case "Director Wilkins":
                return directorPreviousConversations;
            case "Agent Frei":
                return freiPreviousConversations;
            case "Bonsai":
                return bonsaiPreviousConversations;
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

        if(freiQueue.Count + freiPreviousConversations.Count > 0)
        {
            contacts.Add(agentFrei);
            if(freiQueue.Count > 0)
            {
                newMessages.Add(agentFrei);
            }
        }

        if(bonsaiQueue.Count + bonsaiPreviousConversations.Count > 0)
        {
            contacts.Add(bonsai);
            if(bonsaiQueue.Count > 0)
            {
                newMessages.Add(bonsai);
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

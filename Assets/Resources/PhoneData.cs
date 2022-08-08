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
    string QuestionMarks = "??????";
    public List<int> QuestionMarksQueue;
    public List<int> QuestionMarksPreviousConversations;
    string HeadOfIT = "Head of IT";
    public List<int> HeadOfITQueue;
    public List<int> HeadOfITPreviousConversations;

    public List<string> GetActiveContacts()
    {
        List<string> activeContacts = new List<string>();

        if(ODPARCHMENTQueue.Count > 0)
        {
            activeContacts.Add(ODPARCHMENT);
        }

        if(ODTRENCHQueue.Count > 0)
        {
            activeContacts.Add(ODTRENCH);
        }

        if(QuestionMarksQueue.Count > 0)
        {
            activeContacts.Add(QuestionMarks);
        }

        if(HeadOfITQueue.Count > 0)
        {
            activeContacts.Add(HeadOfIT);
        }

        return activeContacts;
    }
}

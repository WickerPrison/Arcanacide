using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PhoneData : ScriptableObject
{
    string meeseeks = "Mr. Meeseeks";
    public List<int> meeseeksQueue;
    string stick = "Stick";
    public List<int> stickQueue;

    public List<string> GetActiveContacts()
    {
        List<string> activeContacts = new List<string>();
        if(meeseeksQueue.Count > 0)
        {
            activeContacts.Add(meeseeks);
        }

        if(stickQueue.Count > 0)
        {
            activeContacts.Add(stick);
        }


        return activeContacts;
    }
}

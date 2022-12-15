using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVparser : MonoBehaviour
{
    public List<List<string>> ParseConversation(TextAsset csvFile)
    {
        List<List<string>> conversations = new List<List<string>>();
        string[] conversationsArray = csvFile.text.Split(';');

        for (int i = 0; i < conversationsArray.Length; i++)
        {
            List<string> conversation = new List<string>();
            string[] test = conversationsArray[i].Split('\n');
            for (int n = 1; n < test.Length - 1; n++)
            {
                conversation.Add(test[n]);
            }
            conversations.Add(conversation);
        }

        return conversations;
    }
}

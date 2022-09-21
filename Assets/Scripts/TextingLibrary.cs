using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLibrary : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] TextAsset ODPARCHMENTtexts;
    [SerializeField] TextAsset ODTRENCHtexts;
    [SerializeField] TextAsset UnknownNumberTexts;
    [SerializeField] TextAsset HeadOfITtexts;

    List<List<string>> conversations = new List<List<string>>();

    public List<List<string>> GetConversations(string contactName, TextingScreen textingScreen)
    {
        switch (contactName)
        {
            case "ODPARCHMENT":
                SetUpConversation(ODPARCHMENTtexts);
                textingScreen.previousConversations = phoneData.ODPARCHMENTPreviousConversations;
                textingScreen.conversationQueue = phoneData.ODPARCHMENTQueue;
                break;
            case "ODTRENCH":
                SetUpConversation(ODTRENCHtexts);
                textingScreen.previousConversations = phoneData.ODTRENCHPreviousConversations;
                textingScreen.conversationQueue = phoneData.ODTRENCHQueue;
                break;
            case "Unknown Number":
                SetUpConversation(UnknownNumberTexts);
                textingScreen.previousConversations = phoneData.UnknownNumberPreviousConversations;
                textingScreen.conversationQueue = phoneData.UnknownNumberQueue;
                break;
            case "Head of IT":
                SetUpConversation(HeadOfITtexts);
                textingScreen.previousConversations = phoneData.HeadOfITPreviousConversations;
                textingScreen.conversationQueue = phoneData.HeadOfITQueue;
                break;
        }

        return conversations;
    }


    public void AddToQueue(string contactName, int conversationIndex)
    {
        switch (contactName)
        {
            case "ODPARCHMENT":
                if (conversationIndex == 0)
                {
                    phoneData.ODTRENCHQueue.Add(0);
                }
                break;
            case "Head of IT":
                if (!phoneData.ODPARCHMENTQueue.Contains(2) && !phoneData.ODPARCHMENTPreviousConversations.Contains(2))
                {
                    phoneData.ODPARCHMENTQueue.Add(2);
                }
                break;
        }
    }

    void SetUpConversation(TextAsset csvFile)
    {
        string[] conversationsArray = csvFile.text.Split(';');

        for (int i = 0; i < conversationsArray.Length; i++)
        {
            List<string> conversation = new List<string>();
            string[] test = conversationsArray[i].Split('\n');
            for(int n = 1; n < test.Length - 1; n++)
            {
                conversation.Add(test[n]);
            }
            conversations.Add(conversation);
        }
    }
}

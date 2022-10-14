using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLibrary : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] TextAsset ORTHODOXtexts;
    [SerializeField] TextAsset TRENCHtexts;
    [SerializeField] TextAsset UnknownNumberTexts;
    [SerializeField] TextAsset HeadOfITtexts;

    List<List<string>> conversations = new List<List<string>>();

    public List<List<string>> GetConversations(string contactName, TextingScreen textingScreen)
    {
        switch (contactName)
        {
            case "ORTHODOX":
                SetUpConversation(ORTHODOXtexts);
                textingScreen.previousConversations = phoneData.ORTHODOXPreviousConversations;
                textingScreen.conversationQueue = phoneData.ORTHODOXQueue;
                break;
            case "TRENCH":
                SetUpConversation(TRENCHtexts);
                textingScreen.previousConversations = phoneData.TRENCHPreviousConversations;
                textingScreen.conversationQueue = phoneData.TRENCHQueue;
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
            case "ORTHODOX":
                if (conversationIndex == 0)
                {
                    phoneData.TRENCHQueue.Add(0);
                }
                break;
            case "Head of IT":
                if (!phoneData.ORTHODOXQueue.Contains(2) && !phoneData.ORTHODOXPreviousConversations.Contains(2))
                {
                    phoneData.ORTHODOXQueue.Add(2);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLibrary : MonoBehaviour
{
    [SerializeField] DialogueData phoneData;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] TextAsset ORTHODOXtexts;
    [SerializeField] TextAsset UnknownNumberTexts;

    List<List<string>> conversations = new List<List<string>>();

    public List<List<string>> GetConversations(string contactName, TextingScreen textingScreen)
    {
        switch (contactName)
        {
            case "Director Wilkins":
                SetUpConversation(ORTHODOXtexts);
                textingScreen.previousConversations = phoneData.directorPreviousConversations;
                textingScreen.conversationQueue = phoneData.directorQueue;
                break;
            case "Unknown Number":
                SetUpConversation(UnknownNumberTexts);
                textingScreen.previousConversations = phoneData.UnknownNumberPreviousConversations;
                textingScreen.conversationQueue = phoneData.UnknownNumberQueue;
                break;
        }

        return conversations;
    }


    public void AddToQueue(string contactName, int conversationIndex)
    {
        //switch (contactName)
        //{
        //    case "ORTHODOX":
        //        if (conversationIndex == 0)
        //        {
        //            phoneData.TRENCHQueue.Add(0);
        //        }
        //        break;
        //}
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

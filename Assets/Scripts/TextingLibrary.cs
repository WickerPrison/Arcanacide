using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLibrary : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] TextAsset directorWilkinsTexts;
    [SerializeField] TextAsset agentFreiTexts;
    [SerializeField] TextAsset unknownNumberTexts;

    List<List<string>> conversations = new List<List<string>>();

    public List<List<string>> GetConversations(string contactName, TextingScreen textingScreen)
    {
        switch (contactName)
        {
            case "Director Wilkins":
                SetUpConversation(directorWilkinsTexts);
                textingScreen.previousConversations = dialogueData.directorPreviousConversations;
                textingScreen.conversationQueue = dialogueData.directorQueue;
                break;
            case "Agent Frei":
                SetUpConversation(agentFreiTexts);
                textingScreen.previousConversations = dialogueData.freiPreviousConversations;
                textingScreen.conversationQueue = dialogueData.freiQueue;
                break;
            case "Unknown Number":
                SetUpConversation(unknownNumberTexts);
                textingScreen.previousConversations = dialogueData.UnknownNumberPreviousConversations;
                textingScreen.conversationQueue = dialogueData.UnknownNumberQueue;
                break;
        }

        return conversations;
    }


    public void AddToQueue(string contactName, int conversationIndex)
    {
        switch (contactName)
        {
            case "Agent Frei":
                if (conversationIndex == 0)
                {
                    dialogueData.directorQueue.Add(2);
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

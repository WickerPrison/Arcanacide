using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextingLibrary : MonoBehaviour
{
    [SerializeField] DialogueData dialogueData;
    [SerializeField] GameObject dialogueBox;

    Dictionary<PhoneContacts, TextAsset> textsDict = new Dictionary<PhoneContacts, TextAsset>();
    [SerializeField] TextAsset directorWilkinsTexts;
    [SerializeField] TextAsset agentFreiTexts;
    [SerializeField] TextAsset bonsaiTexts;
    [SerializeField] TextAsset unknownNumberTexts;

    List<List<string>> conversations = new List<List<string>>();

    private void Awake()
    {
        textsDict = new Dictionary<PhoneContacts, TextAsset>()
        {
            {PhoneContacts.DIRECTORWILKINS, directorWilkinsTexts},
            {PhoneContacts.AGENTFREI, agentFreiTexts},
            {PhoneContacts.SMACKGPT, bonsaiTexts },
            {PhoneContacts.UNKNOWNNUMBER, unknownNumberTexts }
        };
    }

    public List<List<string>> GetConversations(PhoneContacts contactName, TextingScreen textingScreen)
    {
        SetUpConversation(textsDict[contactName]);
        textingScreen.previousConversations = dialogueData.GetPreviousConversations(contactName);
        textingScreen.conversationQueue = dialogueData.GetQueue(contactName);
        return conversations;
    }


    public void AddToQueue(PhoneContacts contactName, int conversationIndex)
    {
        switch (contactName)
        {
            case PhoneContacts.AGENTFREI:
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

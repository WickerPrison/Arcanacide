using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneLibrary : MonoBehaviour
{
    [SerializeField] PhoneData phoneData;
    [SerializeField] GameObject dialogueBox;
    DialogueScript dialogueScript;
    Phone phoneScript;
    List<string> currentConversation;

    List<List<string>> meeseeksConversations = new List<List<string>>();
    List<string> meeseeksConversation0 = new List<string>();
    List<string> meeseeksConversation1 = new List<string>();
    string meeseeks0_0 = "I'm Mr. Meeseeks";
    string meeseeks0_1 = "Look at me";
    string meeseeks1_0 = "Existance is pain to a Meeseeks";

    List<List<string>> stickConversations = new List<List<string>>();
    List<string> stickConversation0 = new List<string>();
    string stick0_0 = "I am a stick";

    private void Start()
    {
        phoneScript = GetComponent<Phone>();

        meeseeksConversation0.Add(meeseeks0_0);
        meeseeksConversation0.Add(meeseeks0_1);
        meeseeksConversation1.Add(meeseeks1_0);
        meeseeksConversations.Add(meeseeksConversation0);
        meeseeksConversations.Add(meeseeksConversation1);

        stickConversation0.Add(stick0_0);
        stickConversations.Add(stickConversation0);
    }

    public void StartConversation(string contactName)
    {
        phoneScript.StartDialogue();
        dialogueScript = Instantiate(dialogueBox).GetComponent<DialogueScript>();
        switch (contactName)
        {
            case "Mr. Meeseeks":
                MrMeeseeksNextConversation();
                break;
            case "Stick":
                StickNextConversation();
                break;
        }
    }

    public void MrMeeseeksNextConversation()
    {
        dialogueScript.SetImage("Patchwork Gary");
        currentConversation = new List<string>(meeseeksConversations[phoneData.meeseeksQueue[0]]);
        dialogueScript.SetText(currentConversation[0]);
        currentConversation.RemoveAt(0);
        phoneData.meeseeksQueue.RemoveAt(0);
    }

    public void StickNextConversation()
    {
        dialogueScript.SetImage("Ernie");
        currentConversation = new List<string>(stickConversations[phoneData.stickQueue[0]]);
        dialogueScript.SetText(currentConversation[0]);
        currentConversation.RemoveAt(0);
        phoneData.stickQueue.RemoveAt(0);
    }

    public void NextDialogue()
    {
        if(currentConversation.Count > 0)
        {
            dialogueScript.SetText(currentConversation[0]);
            currentConversation.RemoveAt(0);
        }
        else
        {
            Destroy(dialogueScript.gameObject);
            phoneScript.EndDialogue();
        }
    }
}

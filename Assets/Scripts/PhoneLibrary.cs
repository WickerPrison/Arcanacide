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

    
    // ODPARCHMENT conversations
    List<List<string>> ODPARCHMENTConversations = new List<List<string>>();

    //Available from beginning of game
    List<string> ODPARCHMENTConversation0 = new List<string>();
    string ODPARCHMENT0_0 = "Player+I’m in.";
    string ODPARCHMENT0_1 = "ODPARCHMENT+Good. Contact the whistleblower for more information, codename: ODTRENCH.";

    //Available after first death
    List<string> ODPARCHMENTConversation1 = new List<string>();
    string ODPARCHMENT1_0 = "Player+The employees here are hostile. They also seem to be drawing power from an arcane being.";
    string ODPARCHMENT1_1 = "ODPARCHMENT+Nothing our division hasn’t dealt with before. We have enough arcane energy stored from the last arcane being we killed to bring you back when you die.";
    string ODPARCHMENT1_2 = "ODPARCHMENT+Have you found any evidence of the previous agent?";
    string ODPARCHMENT1_3 = "Player+Nothing so far.";
    string ODPARCHMENT1_4 = "ODPARCHMENT+Keep me updated.";

    //Available after interacting with Head of IT
    List<string> ODPARCHMENTConversation2 = new List<string>();
    string ODPARCHMENT2_0 = "Player+It’s not just the CEO we have to worry about. I think the head of the IT department also needs to be taken down.";
    string ODPARCHMENT2_1 = "ODPARCHMENT+Very well.";


    // ODTRENCH conversations
    List<List<string>> ODTRENCHConversations = new List<List<string>>();

    //Available after conversation 0 with ODPARCHMENT
    List<string> ODTRENCHConversation0 = new List<string>();
    string ODTRENCH0_0 = "ODTRENCH+The one who started all this is the CEO. You’ll find him on the fourth floor.";
    string ODTRENCH0_1 = "ODTRENCH+Be careful though, it seems like almost everyone here has some sort of crime they want to keep hidden from you. Getting to the fourth floor might be difficult.";
    string ODTRENCH0_2 = "Player+Don’t worry about me. Keep yourself safe and contact me with any new information you find.";

    //Available after getting to FireHub2
    List<string> ODTRENCHConversation1 = new List<string>();
    string ODTRENCH1_0 = "ODTRENCH+To get to the second floor you’ll have to get past the head of the IT department, but he won’t talk to anyone unless they file a support ticket first.";
    string ODTRENCH1_1 = "ODTRENCH+I know there is a computer somewhere on the first floor where you can file a ticket, but I’m not sure where.";


    // ?????? conversations
    List<List<string>> QuestionMarksConversations = new List<List<string>>();

    //Available after getting to Fire Hub 1
    List<string> QuestionMarksConversation0 = new List<string>();
    string QuestionMarks0_0 = "??????+I am too old to grow a beard.";
    string QuestionMarks0_1 = "Player+What? Who is this?";
    string QuestionMarks0_2 = "??????+Let’s all be unique together until we realize we are all the same.";
    string QuestionMarks0_3 = "Player+How are you contacting me?";
    string QuestionMarks0_4 = "??????+…";

    //Available after filing a ticket
    List<string> QuestionMarksConversation1 = new List<string>();
    string QuestionMarks1_0 = "??????+I became paranoid that the school of jellyfish was spying on me.";
    string QuestionMarks1_1 = "Player+…";
    string QuestionMarks1_2 = "??????+Twin 4-month-olds slept in the shade of the palm tree while the mother tanned in the sun.";


    // Head of IT conversations 
    List<List<string>> HeadOfITconversations = new List<List<string>>();

    //Available after killing 4 enemies
    List<string> HeadOfITConversation0 = new List<string>();
    string HeadOfIT0_0 = "Head of IT+This is your first and final warning. Leave now or you will end up like the last agent they sent.";

    //Available after killing 10 enemies
    List<string> HeadOfITConversation1 = new List<string>();
    string HeadOfIT1_0 = "Head of IT+I don’t know why you are still trying. You won’t be able to stop me.";

    //Available after a few deaths
    List<string> HeadOfITConversation2 = new List<string>();
    string HeadOfIT2_0 = "Head of IT+I see your agency has access to some arcane powers as well. It won’t save you.";
    string HeadOfIT2_1 = "Head of IT+I will kill you as many times as it takes for you to stay dead.";

    //Available after a lot of deaths
    List<string> HeadOfITConversation3 = new List<string>();
    string HeadOfIT3_0 = "Head of IT+WHY WON’T YOU STAY DEAD?!";

    //Available after filing a ticket
    List<string> HeadOfITConversation4 = new List<string>();
    string HeadOfIT4_0 = "Head of IT+You managed to file a support ticket.";
    string HeadOfIT4_1 = "Head of IT+Very well. Come and fight me.";


    private void Start()
    {
        phoneScript = GetComponent<Phone>();

        SetUpODPARCHMENTconversations();
        SetUpODTRENCHconversations();
        SetUpQuestionMarksConversations();
        SetUpHeadOfITconversations();
    }

    public void StartConversation(string contactName)
    {
        phoneScript.StartDialogue();
        dialogueScript = Instantiate(dialogueBox).GetComponent<DialogueScript>();
        switch (contactName)
        {
            case "ODPARCHMENT":
                currentConversation = new List<string>(ODPARCHMENTConversations[phoneData.ODPARCHMENTQueue[0]]);
                if(phoneData.ODPARCHMENTQueue[0] == 0)
                {
                    phoneData.ODTRENCHQueue.Add(0);
                }
                phoneData.ODPARCHMENTPreviousConversations.Add(phoneData.ODPARCHMENTQueue[0]);
                phoneData.ODPARCHMENTQueue.RemoveAt(0);
                break;
            case "ODTRENCH":
                currentConversation = new List<string>(ODTRENCHConversations[phoneData.ODTRENCHQueue[0]]);
                phoneData.ODTRENCHPreviousConversations.Add(phoneData.ODTRENCHQueue[0]);
                phoneData.ODTRENCHQueue.RemoveAt(0);
                break;
            case "??????":
                currentConversation = new List<string>(QuestionMarksConversations[phoneData.QuestionMarksQueue[0]]);
                phoneData.QuestionMarksPreviousConversations.Add(phoneData.QuestionMarksQueue[0]);
                phoneData.QuestionMarksQueue.RemoveAt(0);
                break;
            case "Head of IT":
                currentConversation = new List<string>(HeadOfITconversations[phoneData.HeadOfITQueue[0]]);
                if(!phoneData.ODPARCHMENTQueue.Contains(2) && !phoneData.ODPARCHMENTPreviousConversations.Contains(2))
                {
                    phoneData.ODPARCHMENTQueue.Add(2);
                }
                phoneData.HeadOfITPreviousConversations.Add(phoneData.HeadOfITQueue[0]);
                phoneData.HeadOfITQueue.RemoveAt(0);
                break;
        }

        string[] dialogueLine = currentConversation[0].Split('+');
        dialogueScript.SetImage(dialogueLine[0]);
        dialogueScript.SetText(dialogueLine[1]);
        currentConversation.RemoveAt(0);
    }


    public void NextDialogue()
    {
        if(dialogueScript == null)
        {
            return;
        }

        if(currentConversation.Count > 0)
        {
            string[] dialogueLine = currentConversation[0].Split('+');
            dialogueScript.SetImage(dialogueLine[0]);
            dialogueScript.SetText(dialogueLine[1]);
            currentConversation.RemoveAt(0);
        }
        else
        {
            Destroy(dialogueScript.gameObject);
            phoneScript.EndDialogue();
        }
    }

    void SetUpODPARCHMENTconversations()
    {
        ODPARCHMENTConversation0.Add(ODPARCHMENT0_0);
        ODPARCHMENTConversation0.Add(ODPARCHMENT0_1);

        ODPARCHMENTConversation1.Add(ODPARCHMENT1_0);
        ODPARCHMENTConversation1.Add(ODPARCHMENT1_1);
        ODPARCHMENTConversation1.Add(ODPARCHMENT1_2);
        ODPARCHMENTConversation1.Add(ODPARCHMENT1_3);
        ODPARCHMENTConversation1.Add(ODPARCHMENT1_4);

        ODPARCHMENTConversation2.Add(ODPARCHMENT2_0);
        ODPARCHMENTConversation2.Add(ODPARCHMENT2_1);

        ODPARCHMENTConversations.Add(ODPARCHMENTConversation0);
        ODPARCHMENTConversations.Add(ODPARCHMENTConversation1);
        ODPARCHMENTConversations.Add(ODPARCHMENTConversation2);
    }

    void SetUpODTRENCHconversations()
    {
        ODTRENCHConversation0.Add(ODTRENCH0_0);
        ODTRENCHConversation0.Add(ODTRENCH0_1);
        ODTRENCHConversation0.Add(ODTRENCH0_2);

        ODTRENCHConversation1.Add(ODTRENCH1_0);
        ODTRENCHConversation1.Add(ODTRENCH1_1);

        ODTRENCHConversations.Add(ODTRENCHConversation0);
        ODTRENCHConversations.Add(ODTRENCHConversation1);
    }

    void SetUpQuestionMarksConversations()
    {
        QuestionMarksConversation0.Add(QuestionMarks0_0);
        QuestionMarksConversation0.Add(QuestionMarks0_1);
        QuestionMarksConversation0.Add(QuestionMarks0_2);
        QuestionMarksConversation0.Add(QuestionMarks0_3);
        QuestionMarksConversation0.Add(QuestionMarks0_4);

        QuestionMarksConversation1.Add(QuestionMarks1_0);
        QuestionMarksConversation1.Add(QuestionMarks1_1);
        QuestionMarksConversation1.Add(QuestionMarks1_2); 

        QuestionMarksConversations.Add(QuestionMarksConversation0);
        QuestionMarksConversations.Add(QuestionMarksConversation1);
    }

    void SetUpHeadOfITconversations()
    {
        HeadOfITConversation0.Add(HeadOfIT0_0);

        HeadOfITConversation1.Add(HeadOfIT1_0);

        HeadOfITConversation2.Add(HeadOfIT2_0);
        HeadOfITConversation2.Add(HeadOfIT2_1);

        HeadOfITConversation3.Add(HeadOfIT3_0);

        HeadOfITConversation4.Add(HeadOfIT4_0);
        HeadOfITConversation4.Add(HeadOfIT4_1);

        HeadOfITconversations.Add(HeadOfITConversation0);
        HeadOfITconversations.Add(HeadOfITConversation1);
        HeadOfITconversations.Add(HeadOfITConversation2);
        HeadOfITconversations.Add(HeadOfITConversation3);
        HeadOfITconversations.Add(HeadOfITConversation4);
    }
}

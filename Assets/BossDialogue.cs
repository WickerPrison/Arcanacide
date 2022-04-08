using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    InputManager im;

    string introDialogue = "Hmmm... Looks like you somehow resited my control.\nNo matter. Turning you off and on agian ought to do the trick.";
    string lookUpDialogue = "Now where did I save that incantation?";

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Dialogue.Next.performed += ctx => CloseDialogueBox();
        GameManager gm = im.gameObject.GetComponent<GameManager>();
        gm.awareEnemies += 1;

        im.Dialogue();
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        dialogue.SetImage("Head of IT");
        dialogue.SetText(introDialogue);
    }

    void CloseDialogueBox()
    {
        if(dialogue != null)
        {
            BossController bossController = GetComponent<BossController>();
            bossController.hasSeenPlayer = true;
            Destroy(dialogue.gameObject);
            im.Gameplay();
        }
    }

    public void LookUpDialogue()
    {
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        dialogue.SetImage("Head of IT");
        dialogue.SetText(lookUpDialogue);
    }

    public void EndLookUpDialogue()
    {
        if(dialogue != null)
        {
            Destroy(dialogue.gameObject);
        }
    }
}

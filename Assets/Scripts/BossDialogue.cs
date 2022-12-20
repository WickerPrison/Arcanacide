using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;

    string lookUpDialogue = "Now where did I save that incantation?";

    private void Start()
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gm.awareEnemies += 1;
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

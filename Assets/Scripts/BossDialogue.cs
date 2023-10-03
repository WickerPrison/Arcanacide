using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDialogue : MonoBehaviour
{
    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    [SerializeField] string[] lookUpDialogue;
    int randInt = 0;

    public void LookUpDialogue()
    {
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        dialogue.SetImage("Head of IT");
        dialogue.SetText(lookUpDialogue[randInt]);
        randInt = Random.Range(0, 3);
    }

    public void EndLookUpDialogue()
    {
        if(dialogue != null)
        {
            Destroy(dialogue.gameObject);
        }
    }
}

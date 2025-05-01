using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject endDialogueObject;
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] TextAsset CSVfile;
    [SerializeField] string conversationName;
    public int conversationNum;
    [SerializeField] bool stopEnemy;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] bool repeatable = false;
    NavMeshAgent navAgent;
    EnemyController enemyController;
    float speed;
    InputManager im;
    List<List<string>> conversations = new List<List<string>>();
    List<string> thisConversation;
    [System.NonSerialized] public int currentLineIndex = 0;
    DialogueScript dialogueBox;
    bool inDialogue = false;
    Action callback;

    public virtual void Start()
    {
        conversations = CSVparser.ParseConversation(CSVfile);
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        SetConversation(conversationNum);
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        if (stopEnemy)
        {
            navAgent = GetComponentInParent<NavMeshAgent>();
            speed = navAgent.speed;
            enemyController = GetComponentInParent<EnemyController>();
        }
    }

    public void SetConversation(int num)
    {
        thisConversation = conversations[num];
        currentLineIndex = 0;
    }

    public void StartWithCallback(Action callbackFunction)
    {
        callback = callbackFunction;
        StartConversation();
    }

    public void StartConversation()
    {
        if (!repeatable)
        {
            if (dialogueData.conversationsHad.Contains(conversationName))
            {
                return;
            }
            else
            {
                dialogueData.conversationsHad.Add(conversationName);
            }
        }


        im.Dialogue();
        inDialogue = true;
        dialogueBox = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        string[] currentLine = thisConversation[currentLineIndex].Split('|');
        dialogueBox.SetImage(currentLine[0]);
        dialogueBox.SetText(currentLine[1]);
        if (stopEnemy)
        {
            navAgent.speed = 0;
        }
    }

    public virtual void NextLine()
    {
        if (!inDialogue)
        {
            return;
        }

        currentLineIndex++;
        if(currentLineIndex >= thisConversation.Count)
        {
            CloseDialogue();
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogueBox.SetImage(currentLine[0]);
            dialogueBox.SetText(currentLine[1]);
        }
    }

    public void CloseDialogue()
    {
        Destroy(dialogueBox.gameObject);
        inDialogue = false;
        im.Gameplay();
        if (stopEnemy)
        {
            navAgent.speed = speed;
        }

        EndDialogue();
    }

    public virtual void EndDialogue()
    {
        if(callback != null)
        {
            callback();
        }

        if (endDialogueObject != null)
        {
            IEndDialogue endDialogue = endDialogueObject.GetComponent<IEndDialogue>();
            endDialogue.EndDialogue();
        }
    }
}

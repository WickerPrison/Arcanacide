using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] TextAsset CSVfile;
    [SerializeField] int conversationNumber;
    [SerializeField] bool stopEnemy;
    [SerializeField] MapData mapData;
    NavMeshAgent navAgent;
    float speed;
    InputManager im;
    List<List<string>> conversations = new List<List<string>>();
    List<string> thisConversation;
    CSVparser readCSV;
    int currentLineIndex = 0;
    DialogueScript dialogueBox;
    bool inDialogue = false;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        readCSV = GetComponent<CSVparser>();
        conversations = readCSV.ParseConversation(CSVfile);
        thisConversation = conversations[conversationNumber];
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        if (stopEnemy)
        {
            navAgent = GetComponentInParent<NavMeshAgent>();
            speed = navAgent.speed;
        }
    }

    public void StartConversation()
    {
        if (mapData.conversationsHad.Contains(conversationNumber))
        {
            return;
        }

        mapData.conversationsHad.Add(conversationNumber);
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

    void NextLine()
    {
        if (!inDialogue)
        {
            return;
        }

        currentLineIndex++;
        if(currentLineIndex >= thisConversation.Count)
        {
            Destroy(dialogueBox.gameObject);
            inDialogue = false;
            im.Gameplay();
            if (stopEnemy)
            {
                navAgent.speed = speed;
            }
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogueBox.SetImage(currentLine[0]);
            dialogueBox.SetText(currentLine[1]);
        }
    }
}

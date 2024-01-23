using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frei : MonoBehaviour
{
    // input in inspector
    [SerializeField] GameObject message;
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] TextAsset csvFile;
    [SerializeField] int firstTimeConversation;

    //Setup
    InputManager im;
    Transform player;

    //used for dialogue
    DialogueScript dialogue;
    List<List<string>> conversations = new List<List<string>>();
    List<string> thisConversation;
    bool inDialogue = false;
    int conversationIndex;
    int currentLineIndex = 0;

    //other variables
    float playerDistance = 100;
    float interactDistance = 2;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartConversation();
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        conversations = CSVparser.ParseConversation(csvFile);
    }

    void StartConversation()
    {
        if (playerDistance > interactDistance || dialogueData.freiConversations.Contains(firstTimeConversation))
        {
            return;
        }

        conversationIndex = firstTimeConversation;
        dialogueData.freiConversations.Add(firstTimeConversation);
        
        currentLineIndex = 0;
        im.Dialogue();
        inDialogue = true;
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        thisConversation = conversations[conversationIndex];
        string[] currentLine = thisConversation[currentLineIndex].Split('|');
        dialogue.SetImage(currentLine[0]);
        dialogue.SetText(currentLine[1]);
    }

    void NextLine()
    {
        if (!inDialogue)
        {
            return;
        }

        currentLineIndex++;
        if (currentLineIndex >= thisConversation.Count)
        {
            Destroy(dialogue.gameObject);
            dialogueData.directorQueue.Add(3);
            inDialogue = false;
            im.Gameplay();
            Destroy(gameObject);
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogue.SetImage(currentLine[0]);
            dialogue.SetText(currentLine[1]);
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }
}

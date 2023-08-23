using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Whistleblower : MonoBehaviour
{
    // input in inspector
    [SerializeField] GameObject message;
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] TextAsset csvFile;
    [SerializeField] int firstTimeConversation;
    [SerializeField] int repeatableConversation;

    //Setup
    CSVparser readCSV;
    InputManager im;
    Transform player;
    Animator animator;
    Vector3 scale;

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
        readCSV = GetComponent<CSVparser>();
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartConversation();
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        animator = GetComponentInChildren<Animator>();
        scale = animator.transform.localScale;
        conversations = readCSV.ParseConversation(csvFile);
    }

    void StartConversation()
    {
        if (playerDistance > interactDistance)
        {
            return;
        }

        if (dialogueData.whistleBlowerConversations.Contains(firstTimeConversation))
        {
            conversationIndex = repeatableConversation;
        }
        else
        {
            conversationIndex = firstTimeConversation;
            dialogueData.whistleBlowerConversations.Add(firstTimeConversation);
        }

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
            inDialogue = false;
            im.Gameplay();
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
        FacePlayer();

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

    void FacePlayer()
    {
        if (player.position.x > transform.position.x)
        {
            animator.transform.localScale = scale;
        }
        else
        {
            animator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}

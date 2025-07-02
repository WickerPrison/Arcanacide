using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] TextAsset csvFile;
    [SerializeField] GameObject message;
    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    InputManager im;
    Transform player;
    float playerDistance = 100;
    float interactDistance = 2;
    public event EventHandler onStartDialogue;
    List<List<string>> conversations = new List<List<string>>();
    Func<int> GetConversationIndex;
    public Func<int> getConversationIndex
    {
        get
        {
            if (GetConversationIndex != null) return GetConversationIndex;
            else return () => { return 0; };
        }
        set{ GetConversationIndex = value; }
    }
    Action<int> EndConversationCallback;
    public Action<int> endConversationCallback
    {
        get
        {
            if (EndConversationCallback != null) return EndConversationCallback;
            else return x => { };
        }
        set { EndConversationCallback = value; }
    }
    int conversationIndex;
    int currentLineIndex = 0;
    bool inDialogue = false;
    List<string> thisConversation;

    private void Start()
    {
        conversations = CSVparser.ParseConversation(csvFile);
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartConversation();
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
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

    void StartConversation()
    {
        if (playerDistance > interactDistance) return;

        onStartDialogue?.Invoke(this, EventArgs.Empty);
        currentLineIndex = 0;
        im.Dialogue();
        inDialogue = true;
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        conversationIndex = getConversationIndex();
        thisConversation = conversations[conversationIndex];
        string[] currentLine = thisConversation[currentLineIndex].Split('|');
        dialogue.SetImage(currentLine[0]);
        dialogue.SetText(currentLine[1]);
    }

    void NextLine()
    {
        if (!inDialogue) return;

        currentLineIndex++;
        if (currentLineIndex >= thisConversation.Count)
        {
            inDialogue = false;
            im.Gameplay();
            endConversationCallback(conversationIndex);
            Destroy(dialogue.gameObject);
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogue.SetImage(currentLine[0]);
            dialogue.SetText(currentLine[1]);
        }
    }
}

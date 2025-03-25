using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] GameObject shopWindowPrefab;
    [SerializeField] TextAsset csvFile;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] int firstTimeTalking;
    [SerializeField] int alternateFirstTimeTalking;
    [SerializeField] int repeatableGreeting;
    [SerializeField] int farewell;
    [System.NonSerialized] public bool useAlternate = false;
    GameObject shopWindow;
    Transform player;
    InputManager im;
    SoundManager sm;
    float playerDistance = 100;
    float interactDistance = 2;

    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    [SerializeField] List<Patches> patchNames;
    [SerializeField] List<int> emblemCosts;
    List<List<string>> conversations = new List<List<string>>();
    List<string> thisConversation;
    int currentLineIndex = 0;
    int conversationIndex;
    bool inDialogue = false;

    void Start()
    {
        conversations = CSVparser.ParseConversation(csvFile);
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        sm = im.gameObject.GetComponent<SoundManager>();
        im.controls.Gameplay.Interact.performed += ctx => WhichStartingConversation();
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

    void WhichStartingConversation()
    {
        if (dialogueData.patchworkGaryConversations.Contains(firstTimeTalking))
        {
            StartConversation(repeatableGreeting);
        }
        else
        {
            dialogueData.patchworkGaryConversations.Add(firstTimeTalking);
            if (useAlternate)
            {
                StartConversation(alternateFirstTimeTalking);
            }
            else
            {
                StartConversation(firstTimeTalking);
            }
        }
    }

    void StartConversation(int conversation)
    {
        if (playerDistance > interactDistance)
        {
            return;
        }

        currentLineIndex = 0;
        inDialogue = true;
        im.Dialogue();
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        conversationIndex = conversation;
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
            if(conversationIndex == repeatableGreeting || conversationIndex == firstTimeTalking || conversationIndex == alternateFirstTimeTalking)
            {
                OpenShop();
            }
            else if(conversationIndex == farewell)
            {
                im.Gameplay();
            }
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogue.SetImage(currentLine[0]);
            dialogue.SetText(currentLine[1]);
        }
    }

    void OpenShop()
    {
        if(playerDistance <= interactDistance)
        {
            im.Menu();
            shopWindow = Instantiate(shopWindowPrefab);
            ShopWindow shopWindowScript = shopWindow.GetComponent<ShopWindow>();
            shopWindowScript.patchNames = patchNames;
            shopWindowScript.patchCosts = emblemCosts;
        }
    }

    public void CloseShop()
    {
        Destroy(shopWindow);
        StartConversation(farewell);
    }
}

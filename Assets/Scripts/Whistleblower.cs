using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Whistleblower : MonoBehaviour
{
    // input in inspector
    [SerializeField] GameObject message;
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] TextAsset csvFile;
    [SerializeField] int firstTimeConversation;
    [SerializeField] int repeatableConversation;
    [SerializeField] MapData mapData;

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
        if (mapData.whistleblowerArrested) Destroy(gameObject);
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
            if (firstTimeConversation == 4) StartCoroutine(GetArrested());
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

    IEnumerator GetArrested()
    {
        WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
        Image fadeout = GameObject.FindGameObjectWithTag("Fadeout").GetComponent<Image>();
        float fadeoutTime = 1f;
        float timer = fadeoutTime;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            fadeout.color = new Color(0, 0, 0, 1 - (timer / fadeoutTime));
            yield return endOfFrame;
        }

        transform.position = new Vector3(100, 100, 100);
        mapData.whistleblowerArrested = true;
        yield return new WaitForSeconds(0.5f);

        float fadeInTime = 1f;
        timer = fadeInTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeout.color = new Color(0, 0, 0, timer / fadeInTime);
            yield return endOfFrame;
        }
    }
}

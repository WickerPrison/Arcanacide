using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject message;
    [SerializeField] Animator janitorAnimator;
    [SerializeField] GameObject dialoguePrefab;
    [SerializeField] GameObject respecPrefab;
    [SerializeField] TextAsset csvFile;
    [SerializeField] string ability;
    [SerializeField] int newAbilityConversation;
    [SerializeField] int repeatableConversation;
    CSVparser readCSV;
    DialogueScript dialogue;
    TutorialManager tutorialManager;
    Transform player;
    Vector3 scale;
    InputManager im;
    SoundManager sm;
    float playerDistance = 100;
    float interactDistance = 2;
    List<List<string>> conversations = new List<List<string>>();
    int conversationIndex;
    int currentLineIndex = 0;
    bool inDialogue = false;
    List<string> thisConversation;

    void Start()
    {
        readCSV = GetComponent<CSVparser>();
        conversations = readCSV.ParseConversation(csvFile);
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        sm = im.GetComponent<SoundManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartConversation();
        im.controls.Dialogue.Next.performed += ctx => NextLine();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        scale = janitorAnimator.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);
        janitorAnimator.SetFloat("PlayerDistance", playerDistance);

        if(playerDistance <= 4)
        {
            FacePlayer();
        }

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
        if(playerDistance > interactDistance)
        {
            return;
        }

        currentLineIndex = 0;
        im.Dialogue();
        inDialogue = true;
        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        if (!playerData.unlockedAbilities.Contains(ability))
        {
            conversationIndex = newAbilityConversation;
        }
        else
        {
            conversationIndex = repeatableConversation;
        }
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
            if (!playerData.unlockedAbilities.Contains(ability))
            {
                im.Gameplay();
                GetAbility();
            }
            else
            {
                Instantiate(respecPrefab);
            }
        }
        else
        {
            string[] currentLine = thisConversation[currentLineIndex].Split('|');
            dialogue.SetImage(currentLine[0]);
            dialogue.SetText(currentLine[1]);
        }
    }

    void GetAbility()
    {
        playerData.unlockedAbilities.Add(ability);
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();

        if(ability == "Block")
        {
            playerData.killedEnemiesAtGetShield = playerData.killedEnemiesNum;
        }

        if(ability.Contains("More Patches"))
        {
            playerData.maxPatches++;
            tutorialManager.MorePatchesTutorial();
        }
        else
        {
            tutorialManager.NewAbilityTutorial(ability);
        }
    } 


    void FacePlayer()
    {
        if(player.position.x > transform.position.x)
        {
            janitorAnimator.transform.localScale = scale;
        }
        else
        {
            janitorAnimator.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
}

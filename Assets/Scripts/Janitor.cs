using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject message;
    [SerializeField] Animator janitorAnimator;
    [SerializeField] GameObject dialoguePrefab;
    DialogueScript dialogue;
    int conversationTracker = 0;
    TutorialManager tutorialManager;
    Transform player;
    Vector3 scale;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    string dialogue1 = "You look as if you have maintained your free will. You must have more resilience than most.";
    string dialogue2 = "Let me impart some of my power to you. Hopefully it will keep you alive.";
    string dialogue3 = "Good luck. I'm sure we'll meet agian";

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => StartConversation();
        im.controls.Dialogue.Next.performed += ctx => Talk();
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

        dialogue = Instantiate(dialoguePrefab).GetComponent<DialogueScript>();
        if (!playerData.unlockedAbilities.Contains("Block"))
        {
            dialogue.SetText(dialogue1);
        }
        else
        {
            dialogue.SetText(dialogue3);
        }
        dialogue.SetImage("Ernie");
        im.Dialogue();
    }

    void Talk()
    {
        if(playerDistance <= interactDistance && !playerData.unlockedAbilities.Contains("Block"))
        {
            switch (conversationTracker)
            {
                case 0:
                    conversationTracker += 1;
                    dialogue.SetText(dialogue2);
                    break;
                case 1:
                    Destroy(dialogue.gameObject);
                    GetBlock();
                    break;
            }
        }
        else
        {
            Destroy(dialogue.gameObject);
            im.Gameplay();
        }
    }

    void GetBlock()
    {
        playerData.unlockedAbilities.Add("Block");
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
        tutorialManager.BlockTutorial();
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

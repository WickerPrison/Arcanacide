using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITWorker : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    [SerializeField] FireSuppressionSwitch fireSuppressionSwitch;
    NPCDialogue dialogue;
    FaceDirection faceDirection;
    float animationTimer = 0.5f;
    bool inDialogue = false;
    string firstDialogueName = "IT Worker";
    string finalDialogueName = "Fixed Firewall";

    private void Awake()
    {
        dialogue = GetComponent<NPCDialogue>();
        dialogue.getConversationIndex = GetConversationIndex;
        dialogue.endConversationCallback = EndConversation;
    }

    private void OnEnable()
    {
        dialogue.onStartDialogue += onStartDialogue;
        fireSuppressionSwitch.onFixed += onFixed;
    }


    private void Start()
    {
        if (dialogueData.conversationsHad.Contains(finalDialogueName))
        {
            Destroy(gameObject);
        }
        faceDirection = GetComponent<FaceDirection>();
        if(mapData.fireSuppressionState == FireSuppressionState.FIXED)
        {
            faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
        }
        else
        {
            faceDirection.DirectionalFace(FacingDirections.BACK_LEFT);
        }
    }

    private void Update()
    {
        AnimationTimer();
    }

    void AnimationTimer()
    {
        if (mapData.fireSuppressionState == FireSuppressionState.FIXED || inDialogue) return;
        animationTimer -= Time.deltaTime;
        if (animationTimer <= 0)
        {
            float randFloat = Random.Range(0f, 1f);
            switch (randFloat)
            {
                case float n when n < 0.3f:
                    backAnimator.Play("Hmmm");
                    break;
                case float n when n < 0.6:
                    backAnimator.Play("Smack");
                    break;
                default:
                    backAnimator.Play("ButtonPress");
                    break;
            }
            animationTimer += Random.Range(2.3f, 4.5f);
        }
    }

    int GetConversationIndex()
    {
        (FireSuppressionState, bool) values = (mapData.fireSuppressionState, dialogueData.conversationsHad.Contains(firstDialogueName));
        int conversationIndex = values switch
        {
            (FireSuppressionState.ON, false) => 0,
            (FireSuppressionState.ON, true) => 1,
            (FireSuppressionState.FIXED, false) => 10,
            (FireSuppressionState.FIXED, true) => 2,
            _ => -1
        };

        if(conversationIndex == 0 || conversationIndex == 10)
        {
            dialogueData.conversationsHad.Add(firstDialogueName);
        }

        return conversationIndex;
    }

    void EndConversation(int index)
    {
        inDialogue = false;
        if(index < 2)
        {
            faceDirection.DirectionalFace(FacingDirections.BACK_LEFT);
        }
        else
        {
            faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
            if (!dialogueData.conversationsHad.Contains(finalDialogueName))
            {
                dialogueData.conversationsHad.Add(finalDialogueName);
            }
        }
    }
    
    private void onStartDialogue(object sender, System.EventArgs e)
    {
        inDialogue = true;
        backAnimator.Play("Idle");
        frontAnimator.Play("Idle");
        faceDirection.FacePlayer();
    }

    private void onFixed(object sender, System.EventArgs e)
    {
        backAnimator.Play("Idle");
        frontAnimator.Play("Idle");
        faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
    }

    private void OnDisable()
    {
        dialogue.onStartDialogue -= onStartDialogue;
        fireSuppressionSwitch.onFixed -= onFixed;
    }
}

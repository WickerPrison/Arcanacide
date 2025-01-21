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
        if(mapData.fireSuppressionState == FireSuppressionState.FIXED)
        {
            return 2;
        }

        if (!dialogueData.conversationsHad.Contains(firstDialogueName))
        {
            dialogueData.conversationsHad.Add(firstDialogueName);
            return 0;
        }

        return 1;
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

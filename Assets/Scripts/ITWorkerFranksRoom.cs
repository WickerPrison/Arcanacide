using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITWorkerFranksRoom : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    NPCDialogue dialogue;
    FaceDirection faceDirection;

    private void Awake()
    {
        dialogue = GetComponent<NPCDialogue>();
        dialogue.getConversationIndex = GetConversationIndex;
        dialogue.endConversationCallback = EndConversation;
    }

    private void Start()
    {
        faceDirection = GetComponent<FaceDirection>();
        backAnimator.Play("Idle");
        frontAnimator.Play("Idle");
        faceDirection.DirectionalFace(FacingDirections.FRONT_LEFT);
        mapData.outsideFrankBossfight = true;
    }


    int GetConversationIndex()
    {
        return dialogueData.conversationsHad.Contains(dialogueData.outsideFranksRoom) ? 6 : 5;
    }

    void EndConversation(int index)
    {
        faceDirection.DirectionalFace(FacingDirections.FRONT_LEFT);
        if(index == 5)
        {
            dialogueData.conversationsHad.Add(dialogueData.outsideFranksRoom);
        }
    }

    private void onStartDialogue(object sender, System.EventArgs e)
    {
        faceDirection.FacePlayer();
    }

    private void OnEnable()
    {
        dialogue.onStartDialogue += onStartDialogue;
    }

    private void OnDisable()
    {
        dialogue.onStartDialogue -= onStartDialogue;
    }
}

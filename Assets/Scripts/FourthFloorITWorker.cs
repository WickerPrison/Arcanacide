using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthFloorITWorker : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    NPCDialogue dialogue;
    FaceDirection faceDirection;
    float animationTimer = 0.5f;
    bool inDialogue = false;

    private void Awake()
    {
        dialogue = GetComponent<NPCDialogue>();
        dialogue.getConversationIndex = () => { return mapData.resetPasswords == null ? 7 : 8; };
        dialogue.endConversationCallback = EndConversation;
        if(mapData.resetPasswords.Count == 4)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void OnEnable()
    {
        dialogue.onStartDialogue += onStartDialogue;
    }

    private void Start()
    {
        faceDirection = GetComponent<FaceDirection>();
        faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
    }

    private void Update()
    {
        AnimationTimer();
    }

    void AnimationTimer()
    {
        if (inDialogue) return;
        animationTimer -= Time.deltaTime;
        if (animationTimer <= 0)
        {
            float randFloat = Random.Range(0f, 1f);
            switch (randFloat)
            {
                case float n when n < 0.2f:
                    backAnimator.Play("Hmmm");
                    break;
                default:
                    backAnimator.Play("Keyboard");
                    break;
            }
            animationTimer += Random.Range(5f, 10f);
        }
    }

    void EndConversation(int index)
    {
        inDialogue = false;
        faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
        if (mapData.resetPasswords == null) mapData.resetPasswords = new List<int>();
    }

    private void onStartDialogue(object sender, System.EventArgs e)
    {
        inDialogue = true;
        backAnimator.Play("Idle");
        frontAnimator.Play("Idle");
        faceDirection.FacePlayer();
    }

    private void OnDisable()
    {
        dialogue.onStartDialogue -= onStartDialogue;
    }
}

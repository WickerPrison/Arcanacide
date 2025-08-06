using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteACWorker : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] MapData mapData;
    [SerializeField] DialogueData dialogueData;
    NPCDialogue dialogue;
    FaceDirection faceDirection;
    TutorialManager tutorialManager;
    float animationTimer = 0.5f;
    bool inDialogue = false;

    private void Awake()
    {
        dialogue = GetComponent<NPCDialogue>();
        dialogue.getConversationIndex = () => { return mapData.hasRemoteAC ? 4 : 3; };
        dialogue.endConversationCallback = EndConversation;
    }

    private void OnEnable()
    {
        dialogue.onStartDialogue += onStartDialogue;
    }


    private void Start()
    {
        if (mapData.outsideFrankBossfight)
        {
            Destroy(gameObject);
            return;
        }
        tutorialManager = GlobalEvents.instance.gameObject.GetComponent<TutorialManager>();
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

    void EndConversation(int index)
    {
        inDialogue = false;
        faceDirection.DirectionalFace(FacingDirections.BACK_RIGHT);
        if (index < 4)
        {
            mapData.hasRemoteAC = true;
            tutorialManager.Tutorial(tutorialManager.remoteAC);
        }
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

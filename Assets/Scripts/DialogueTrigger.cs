using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    Transform player;
    Dialogue dialogue;
    float distance;
    [SerializeField] float talkDistance;
    bool hasHadConversation = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dialogue = GetComponent<Dialogue>();
    }

    private void Update()
    {
        if (!hasHadConversation)
        {
            distance = Vector3.Distance(player.position, transform.position);
            if(distance <= talkDistance)
            {
                hasHadConversation = true;
                dialogue.StartConversation();
            }
        }
    }
}

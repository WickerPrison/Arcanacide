using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectEvidence : MonoBehaviour
{
    [SerializeField] string evidenceName;
    [SerializeField] GameObject message;
    [SerializeField] float interactDistance = 2;
    [SerializeField] PlayerData playerData;
    [SerializeField] ParticleSystem wayFaerie;
    Dialogue dialogue;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    bool hasCollectedEvidence = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        dialogue = GetComponent<Dialogue>();

        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Investigate();

        if (playerData.evidenceFound.Contains(evidenceName))
        {
            hasCollectedEvidence = true;
            if(wayFaerie != null)
            {
                wayFaerie.Clear();
                wayFaerie.Stop();
            }
        }
    }

    void Update()
    {
        if(hasCollectedEvidence)
        {
            message.SetActive(false);
            return;
        }
        else if (playerData.evidenceFound.Contains(evidenceName))
        {
            hasCollectedEvidence = true;
            if (wayFaerie != null) wayFaerie.Stop();
            message.SetActive(false);
            return;
        }

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

    void Investigate()
    {
        if(!hasCollectedEvidence && playerDistance <= interactDistance)
        {
            playerData.evidenceFound.Add(evidenceName);
            hasCollectedEvidence = true;
            if(wayFaerie!= null) wayFaerie.Stop();
            dialogue.StartConversation();
        }
    }
}

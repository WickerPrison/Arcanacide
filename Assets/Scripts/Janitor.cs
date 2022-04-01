using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Janitor : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject message;
    [SerializeField] GameObject tutorailPrefab;
    [SerializeField] Animator janitorAnimator;
    Transform player;
    Vector3 scale;
    InputManager im;
    float playerDistance;
    float interactDistance = 2;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Talk();
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

        if (playerDistance <= interactDistance && !playerData.unlockedAbilities.Contains("Block"))
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void Talk()
    {
        if(playerDistance <= interactDistance && !playerData.unlockedAbilities.Contains("Block"))
        {
            playerData.unlockedAbilities.Add("Block");
            im.Tutorial();
            Instantiate(tutorailPrefab);
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

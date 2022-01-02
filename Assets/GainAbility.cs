using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainAbility : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject message;
    [SerializeField] GameObject tutorailPrefab;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2 && !playerData.hasBlock)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerData.hasBlock = true;
                Instantiate(tutorailPrefab);
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}

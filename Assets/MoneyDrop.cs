using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject message;
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
                playerData.money += playerData.lostMoney;
                playerData.lostMoney = 0;
                Destroy(gameObject);
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}

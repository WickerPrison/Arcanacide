using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileTicket : MonoBehaviour
{
    [SerializeField] MapData mapData;
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
        if (playerDistance <= 2 && !mapData.ticketFiled)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                mapData.ticketFiled = true;                
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}

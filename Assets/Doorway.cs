using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : MonoBehaviour
{
    bool doorOpen;
    [SerializeField] string nextRoom;
    [SerializeField] MapData mapData;
    [SerializeField] int doorNumber;
    [SerializeField] GameObject message;
    Transform player;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        doorOpen = true;
    }

    private void Update()
    {
        if(gm.awareEnemies < 1)
        {
            doorOpen = true;
        }
        else
        {
            doorOpen = false;
        }

        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2 && doorOpen)
        {
            message.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                mapData.doorNumber = doorNumber;
                SceneManager.LoadScene(nextRoom);
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}

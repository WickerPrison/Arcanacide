using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneWayDoor : MonoBehaviour
{
    bool doorOpen;
    [SerializeField] string nextRoom;
    [SerializeField] MapData mapData;
    [SerializeField] int doorNumber;
    [SerializeField] GameObject message;
    [SerializeField] GameObject lockedMessage;
    [SerializeField] int lockedDoorID;
    Transform player;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        doorOpen = true;
    }

    public void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            if (gm.awareEnemies < 1 && mapData.unlockedDoors.Contains(lockedDoorID))
            {
                doorOpen = true;
            }
            else
            {
                doorOpen = false;
            }

            if (doorOpen)
            {
                message.SetActive(true);
                lockedMessage.SetActive(false);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    mapData.doorNumber = doorNumber;
                    SceneManager.LoadScene(nextRoom);
                }
            }
            else
            {
                message.SetActive(false);
                if (!mapData.unlockedDoors.Contains(lockedDoorID))
                {
                    lockedMessage.SetActive(true);
                }
            }
        }
        else
        {
            message.SetActive(false);
            lockedMessage.SetActive(false);
        }
    }
}

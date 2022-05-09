using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : MonoBehaviour
{
    public bool doorOpen = false;
    [SerializeField] string nextRoom;
    public MapData mapData;
    [SerializeField] int doorNumber;
    [SerializeField] GameObject message;
    [SerializeField] GameObject lockedMessage;
    [SerializeField] GameObject doorAudioPrefab;
    [SerializeField] int lockedDoorID;
    GameObject doorAudio;
    public Transform player;
    GameManager gm;
    InputManager im;
    public float playerDistance = 100;
    float interactDistance = 2;
    float doorTimer = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => OpenDoor();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        doorOpen = true;
        message.SetActive(false);
    }

    public virtual void Update()
    {
        if(doorTimer > 0)
        {
            doorTimer -= Time.deltaTime;
        }

        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && gm.awareEnemies < 1)
        {
            if (lockedDoorID == 0 || mapData.unlockedDoors.Contains(lockedDoorID))
            {
                doorOpen = true;
                message.SetActive(true);
                lockedMessage.SetActive(false);
            }
            else
            {
                doorOpen = false;
                message.SetActive(false);
                lockedMessage.SetActive(true);
            }
        }
        else
        {
            doorOpen = false;
            message.SetActive(false);
            lockedMessage.SetActive(false);
        }
    }

    void OpenDoor()
    {
        if(doorTimer > 0)
        {
            return;
        }

        if(playerDistance <= interactDistance && doorOpen)
        {
            doorAudio = Instantiate(doorAudioPrefab);
            DontDestroyOnLoad(doorAudio);
            doorAudio.GetComponent<AudioSource>().Play();
            mapData.doorNumber = doorNumber;
            PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
            if (playerScript.duckHealTimer > 0)
            {
                playerScript.MaxHeal();
            }
            SceneManager.LoadScene(nextRoom);
        }
    }

    /*
public virtual void Update()
{
    playerDistance = Vector3.Distance(transform.position, player.position);
    if (playerDistance <= interactDistance)
    {
        if (gm.awareEnemies < 1)
        {
            doorOpen = true;
            message.SetActive(true);
        }
        else
        {
            doorOpen = false;
            message.SetActive(false);
        }
    }
    else
    {
        message.SetActive(false);
    }
}
*/
}

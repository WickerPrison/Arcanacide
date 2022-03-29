using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Doorway : MonoBehaviour
{
    public bool doorOpen;
    [SerializeField] string nextRoom;
    public MapData mapData;
    [SerializeField] int doorNumber;
    [SerializeField] GameObject message;
    [SerializeField] GameObject doorAudioPrefab;
    GameObject doorAudio;
    public Transform player;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        doorOpen = true;
        message.SetActive(false);
    }

    public virtual void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= 2)
        {
            if (gm.awareEnemies < 1)
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    doorAudio = Instantiate(doorAudioPrefab);
                    DontDestroyOnLoad(doorAudio);
                    doorAudio.GetComponent<AudioSource>().Play();
                    mapData.doorNumber = doorNumber;
                    PlayerScript playerScript = player.gameObject.GetComponent<PlayerScript>();
                    if(playerScript.duckHealTimer > 0)
                    {
                        playerScript.MaxHeal();
                    }
                    SceneManager.LoadScene(nextRoom);
                }
            }
            else
            {
                message.SetActive(false);
            }
        }
        else
        {
            message.SetActive(false);
        }
    }
}

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
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        doorOpen = true;    
    }

    private void Update()
    {
        if(gm.numberOfEnemies < 1)
        {
            doorOpen = true;
        }
        else
        {
            doorOpen = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (doorOpen)
            {
                mapData.doorNumber = doorNumber;
                SceneManager.LoadScene(nextRoom);
            }
        }
    }
}

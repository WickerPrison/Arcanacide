using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] Transform spawnPoint;
    [SerializeField] int altarNumber;
    Transform player;
    PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = player.gameObject.GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float playerDistance = Vector3.Distance(transform.position, player.position);
            if(playerDistance <= 2)
            {
                playerData.lastAltar = altarNumber;
                mapData.doorNumber = 0;
                mapData.deadEnemies.Clear();
                playerScript.Rest();
            }
        }
    }
}

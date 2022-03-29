using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestMenuButtons : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject emblemMenuPrefab;
    [SerializeField] GameObject levelUpMenuPrefab;
    GameObject emblemMenu;
    GameObject levelUpMenu;
    public Transform spawnPoint;
    public int altarNumber;
    Transform player;
    PlayerController playerController;
    PlayerScript playerScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.gameObject.GetComponent<PlayerController>();
        playerScript = player.gameObject.GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseRestMenu();
        }
    }

    public void OpenEmblemMenu()
    {
        emblemMenu = Instantiate(emblemMenuPrefab);
        EmblemMenu emblemMenuScript = emblemMenu.GetComponent<EmblemMenu>();
        emblemMenuScript.altarNumber = altarNumber;
        emblemMenuScript.spawnPoint = spawnPoint;
        Destroy(gameObject);
    }

    public void OpenLevelUpMenu()
    {
        levelUpMenu = Instantiate(levelUpMenuPrefab);
        LevelUpMenu levelUpMenuScript = levelUpMenu.GetComponent<LevelUpMenu>();
        levelUpMenuScript.altarNumber = altarNumber;
        levelUpMenuScript.spawnPoint = spawnPoint;
        Destroy(gameObject);
    }

    public void CloseRestMenu()
    {
        playerController.anyMenuOpen = false;
        playerController.preventInput = false;
        Rest();
        Destroy(gameObject);
    }


    void Rest()
    {
        playerData.lastAltar = altarNumber;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.usedChargingStations.Clear();
        playerScript.Rest();
    }
}

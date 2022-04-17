using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RestMenuButtons : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] GameObject emblemMenuPrefab;
    [SerializeField] GameObject levelUpMenuPrefab;
    [SerializeField] GameObject mapMenuPrefab;
    public Vector3 mapPlayerFacePosition;
    public GameObject firstButton;
    GameObject emblemMenu;
    GameObject levelUpMenu;
    GameObject mapMenu;
    public Transform spawnPoint;
    public int altarNumber;
    Transform player;
    PlayerController playerController;
    PlayerScript playerScript;
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => CloseRestMenu();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.gameObject.GetComponent<PlayerController>();
        playerScript = player.gameObject.GetComponent<PlayerScript>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void OpenEmblemMenu()
    {
        emblemMenu = Instantiate(emblemMenuPrefab);
        EmblemMenu emblemMenuScript = emblemMenu.GetComponent<EmblemMenu>();
        emblemMenuScript.altarNumber = altarNumber;
        emblemMenuScript.spawnPoint = spawnPoint;
        emblemMenuScript.restMenuScript = this;
        controls.Disable();
    }

    public void OpenLevelUpMenu()
    {
        levelUpMenu = Instantiate(levelUpMenuPrefab);
        LevelUpMenu levelUpMenuScript = levelUpMenu.GetComponent<LevelUpMenu>();
        levelUpMenuScript.altarNumber = altarNumber;
        levelUpMenuScript.spawnPoint = spawnPoint;
        levelUpMenuScript.restMenuScript = this;
        controls.Disable();
    }

    public void OpenMapMenu()
    {
        mapMenu = Instantiate(mapMenuPrefab);
        MapMenu mapMenuScript = mapMenu.GetComponent<MapMenu>();
        mapMenuScript.restMenuScript = this;
        mapMenuScript.playerFacePosition = mapPlayerFacePosition;
        controls.Disable();
    }

    public void CloseRestMenu()
    {
        Rest();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>().Gameplay();
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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}

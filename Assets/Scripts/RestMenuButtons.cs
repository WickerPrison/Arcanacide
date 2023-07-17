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
    [SerializeField] GameObject textMenuPrefab;
    [SerializeField] GameObject newMessage;
    [SerializeField] DialogueData dialogueData;
    public Vector3 mapPlayerFacePosition;
    public GameObject firstButton;
    GameObject emblemMenu;
    GameObject levelUpMenu;
    GameObject mapMenu;
    GameObject textMenu;
    public Transform spawnPoint;
    public int altarNumber;
    Transform player;
    PlayerMovement playerController;
    PlayerScript playerScript;
    public PlayerControls controls;
    SoundManager sm;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => CloseRestMenu();
    }

    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.gameObject.GetComponent<PlayerMovement>();
        playerScript = player.gameObject.GetComponent<PlayerScript>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void Update()
    {
        if (dialogueData.GetNewMessages().Count == 0)
        {
            newMessage.SetActive(false);
        }
    }

    public void OpenEmblemMenu()
    {
        sm.ButtonSound();
        emblemMenu = Instantiate(emblemMenuPrefab);
        EmblemMenu emblemMenuScript = emblemMenu.GetComponent<EmblemMenu>();
        emblemMenuScript.altarNumber = altarNumber;
        emblemMenuScript.spawnPoint = spawnPoint;
        emblemMenuScript.restMenuScript = this;
        controls.Disable();
    }

    public void OpenLevelUpMenu()
    {
        sm.ButtonSound();
        levelUpMenu = Instantiate(levelUpMenuPrefab);
        LevelUpMenu levelUpMenuScript = levelUpMenu.GetComponent<LevelUpMenu>();
        levelUpMenuScript.altarNumber = altarNumber;
        levelUpMenuScript.spawnPoint = spawnPoint;
        levelUpMenuScript.restMenuScript = this;
        controls.Disable();
    }

    public void OpenTextMenu()
    {
        sm.ButtonSound();
        textMenu = Instantiate(textMenuPrefab);
        textMenu.GetComponent<TextingMenu>().restMenuScript = this;
        controls.Disable();
    }

    public void OpenMapMenu()
    {
        sm.ButtonSound();
        mapMenu = Instantiate(mapMenuPrefab);
        MapMenu mapMenuScript = mapMenu.GetComponent<MapMenu>();
        mapMenuScript.restMenuScript = this;
        mapMenuScript.playerFacePosition = mapPlayerFacePosition;
        controls.Disable();
    }

    public void CloseRestMenu()
    {
        sm.ButtonSound();
        Rest();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>().Gameplay();
        Destroy(gameObject);
    }

    void Rest()
    {
        sm.RestSound();
        playerData.lastSwordSite = altarNumber;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
        mapData.usedAltars.Clear();
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

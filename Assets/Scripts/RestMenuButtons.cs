using FMODUnity;
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
    [SerializeField] GameObject gemMenuPrefab;
    [SerializeField] GameObject refundStoneNew;
    [SerializeField] GameObject mapMenuPrefab;
    [SerializeField] GameObject textMenuPrefab;
    [SerializeField] GameObject textNewMessage;
    [SerializeField] GameObject weaponMenuPrefab;
    [SerializeField] GameObject weaponNew;
    [SerializeField] DialogueData dialogueData;
    public Vector3 mapPlayerFacePosition;
    public GameObject firstButton;
    GameObject emblemMenu;
    GameObject levelUpMenu;
    GameObject gemMenu;
    GameObject mapMenu;
    GameObject textMenu;
    GameObject weaponMenu;
    public Transform spawnPoint;
    public int altarNumber;
    Transform player;
    PlayerMovement playerController;
    PlayerScript playerScript;
    public PlayerControls controls;
    SoundManager sm;
    [SerializeField] EventReference healSFX;

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

        weaponNew.SetActive(playerData.newWeapon);
    }

    private void Update()
    {
        if (dialogueData.GetNewMessages().Count == 0)
        {
            textNewMessage.SetActive(false);
        }

        if (playerData.currentGemShards < 3)
        {
            refundStoneNew.SetActive(false);
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
        levelUpMenu.GetComponent<LevelUpMenu>().restMenuScript = this;
        controls.Disable();
    }

    public void OpenGemMenu()
    {
        sm.ButtonSound();
        Instantiate(gemMenuPrefab).GetComponent<HealingGemMenu>().restMenuScript = this;
        controls.Disable();
    }

    public void OpenTextMenu()
    {
        sm.ButtonSound();
        textMenu = Instantiate(textMenuPrefab);
        textMenu.GetComponent<TextingMenu>().restMenuScript = this;
        controls.Disable();
    }

    public void OpenWeaponMenu()
    {
        sm.ButtonSound();
        weaponMenu = Instantiate(weaponMenuPrefab);
        weaponMenu.GetComponent<WeaponMenu>().restMenu = this;
        weaponNew.SetActive(false);
        playerData.newWeapon = false;
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
        RuntimeManager.PlayOneShot(healSFX, 0.4f);   
        playerData.lastSwordSite = altarNumber;
        mapData.doorNumber = 0;
        mapData.deadEnemies.Clear();
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

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Doorway : MonoBehaviour
{
    public bool doorOpen = false;
    public string nextRoom;
    public MapData mapData;
    public int doorNumber;
    [SerializeField] GameObject message;
    [SerializeField] GameObject lockedMessage;
    [SerializeField] GameObject doorAudioPrefab;
    public int lockedDoorID;
    [SerializeField] Material fogWallMaterial;
    public Transform player;
    GameManager gm;
    InputManager im;
    public float playerDistance = 100;
    float interactDistance = 2;
    float doorTimer = 0.5f;
    bool fogOn = false;
    float fadeDuration = 0.5f;
    SpriteRenderer spriteRenderer;
    [SerializeField] SpriteRenderer fogWallRenderer;
    [SerializeField] EventReference doorSFX;
    [SerializeField] GameObject blockDoorsObject;
    IBlockDoors blockDoors;
    bool doorBlocked;
    public event System.EventHandler OnOpenDoor;


    private void OnValidate()
    {
        if (blockDoorsObject == null) return;
        blockDoors = blockDoorsObject.GetComponent<IBlockDoors>();
        if(blockDoors == null)
        {
            blockDoorsObject = null;
        }
    }

    private void Awake()
    {
        if(blockDoorsObject != null)
        {
            blockDoors = blockDoorsObject.GetComponent<IBlockDoors>();
        }
    }

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => OpenDoor();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        doorOpen = true;
        message.SetActive(false);
        fogWallMaterial.SetFloat("_FadeIn", 0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        fogWallRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
    }

    public virtual void Update()
    {
        if(doorTimer > 0)
        {
            doorTimer -= Time.deltaTime;
        }

        playerDistance = Vector3.Distance(transform.position, player.position);
        if (playerDistance <= interactDistance && gm.awareEnemies < 1 && !doorBlocked)
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

        if (fogOn && gm.awareEnemies <= 0 && !doorBlocked)
        {
            fogOn = false;
            StartCoroutine(FogWallOff());
        }
        else if(!fogOn && (gm.awareEnemies > 0 || doorBlocked))
        {
            fogOn = true;
            StartCoroutine(FogWallOn());
        }
    }

    IEnumerator FogWallOn()
    {
        float fadeTimer = fadeDuration;
        float fadeRatio = fadeTimer / fadeDuration;
        while(fadeTimer > 0)
        {

            fadeTimer -= Time.deltaTime;
            fadeRatio = fadeTimer / fadeDuration;
            fogWallMaterial.SetFloat("_FadeIn", 1 - fadeRatio);
            yield return new WaitForEndOfFrame();
        }
        fogWallMaterial.SetFloat("_FadeIn", 1);
    }

    IEnumerator FogWallOff()
    {
        float fadeTimer = fadeDuration;
        float fadeRatio = fadeTimer / fadeDuration;
        while (fadeTimer > 0)
        {

            fadeTimer -= Time.deltaTime;
            fadeRatio = fadeTimer / fadeDuration;
            fogWallMaterial.SetFloat("_FadeIn", fadeRatio);
            yield return new WaitForEndOfFrame();
        }

        fogWallMaterial.SetFloat("_FadeIn", 0);
    }

    void OpenDoor()
    {
        if(doorTimer > 0)
        {
            return;
        }

        if(playerDistance <= interactDistance && doorOpen)
        {
            OnOpenDoor?.Invoke(this, EventArgs.Empty);
            RuntimeManager.PlayOneShot(doorSFX);
            mapData.doorNumber = doorNumber;
            PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth.gemHealTimer > 0)
            {
                playerHealth.MaxHeal();
            }
            SceneManager.LoadScene(nextRoom);
        }
    }

    private void BlockDoors_onBlockDoor(object sender, bool isBlocked)
    {
        doorBlocked = isBlocked;
    }

    private void OnEnable()
    {
        if(blockDoors != null)
        {
            blockDoors.onBlockDoor += BlockDoors_onBlockDoor;
        }
    }

    private void OnDisable()
    {
        if(blockDoors != null)
        {
            blockDoors.onBlockDoor -= BlockDoors_onBlockDoor;
        }
    }
}

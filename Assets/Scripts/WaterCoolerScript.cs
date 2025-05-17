using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCoolerScript : MonoBehaviour
{
    [SerializeField] int coolerID;
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] Transform water;
    HUD hud;
    bool hasBeenUsed = false;
    Transform player;
    InputManager im;
    //TutorialManager tutorialManager;
    float playerDistance = 100;
    float interactDistance = 2;
    float waterMaxHeight = 0.6f;
    float currentFill = 1;

    void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => Charge();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        hud = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<HUD>();
        if (mapData.usedCoolers.Contains(coolerID))
        {
            hasBeenUsed = true;
            currentFill = 0;
            SetWaterHeight(0);
        }
        else
        {
            SetWaterHeight(1);
        }
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance && !hasBeenUsed)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }

        if(hasBeenUsed && currentFill > 0)
        {
            currentFill -= Time.deltaTime;
            SetWaterHeight(currentFill);
        }
    }

    void Charge()
    {
        if(playerDistance <= interactDistance && !hasBeenUsed)
        {
            playerData.maxMana += 25;
            hasBeenUsed = true;
            mapData.usedCoolers.Add(coolerID);
            hud.MaxManaIncreased();
        }
    }

    void SetWaterHeight(float decimalFull)
    {
        float currentHeight = waterMaxHeight * decimalFull;
        water.localPosition = new Vector3(water.localPosition.x, currentHeight, water.localPosition.z);
        water.localScale = new Vector3(water.localScale.x, currentHeight, water.localScale.z);
    }
}

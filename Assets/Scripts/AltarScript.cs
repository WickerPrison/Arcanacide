using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarScript : MonoBehaviour
{
    [SerializeField] int altarID;
    [SerializeField] GameObject message;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] Transform water;
    bool hasBeenUsed = false;
    Transform player;
    PlayerHealth playerHealth;
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
        playerHealth = player.GetComponent<PlayerHealth>();
        if (mapData.usedAltars.Contains(altarID))
        {
            hasBeenUsed = true;
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
        if(playerData.health == playerData.MaxHealth())
        {
            return;
        }

        if(playerDistance <= interactDistance && !hasBeenUsed)
        {
            playerHealth.MaxHeal();
            hasBeenUsed = true;
            mapData.usedAltars.Add(altarID);
        }
    }

    void SetWaterHeight(float decimalFull)
    {
        float currentHeight = waterMaxHeight * decimalFull;
        water.localPosition = new Vector3(water.localPosition.x, currentHeight, water.localPosition.z);
        water.localScale = new Vector3(water.localScale.x, currentHeight, water.localScale.z);
    }
}

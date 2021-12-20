using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckIcons : MonoBehaviour
{
    [SerializeField] Image heartIconFilled;
    [SerializeField] Image heartIconEmpty;
    [SerializeField] Image blockIconFilled;
    [SerializeField] Image blockIconEmpty;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        DisableAllIcons();
        if(playerController.duckCD <= 0)
        {
            FilledIcons();
        }
        else
        {
            EmptyIcons();
        }
    }

    void FilledIcons()
    {
        switch (playerController.equippedAbility)
        {
            case "Heal":
                heartIconFilled.enabled = true;
                break;
            case "Block":
                blockIconFilled.enabled = true;
                break;
        }
    }

    void EmptyIcons()
    {
        switch (playerController.equippedAbility)
        {
            case "Heal":
                heartIconEmpty.enabled = true;
                break;
            case "Block":
                blockIconEmpty.enabled = true;
                break;
        }
    }

    void DisableAllIcons()
    {
        heartIconEmpty.enabled = false;
        heartIconFilled.enabled = false;
        blockIconEmpty.enabled = false;
        blockIconFilled.enabled = false;
    }
}

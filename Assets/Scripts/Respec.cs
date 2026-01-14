using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Respec : MonoBehaviour
{
    SoundManager sm;
    InputManager im;
    [SerializeField] GameObject noButton;
    [SerializeField] PlayerData playerData;
    PlayerScript playerScript;
    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Menu.Back.started += ctx => ResumeGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
        im = sm.GetComponent<InputManager>();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(noButton);
    }

    public void ResumeGame()
    {
        sm.ButtonSound();
        im.Gameplay();
        Destroy(gameObject);
    }

    public void RespecButton()
    {
        sm.ButtonSound();
        im.Gameplay();

        int level = playerData.GetLevel();
        for(int i = level - 1; i > 0; i--)
        {
            playerData.money += Mathf.RoundToInt(4 + Mathf.Pow(i, 2));
        }

        playerData.strength = 1;
        playerData.dexterity = 1;
        playerData.vitality = 1;
        playerData.arcane = 1;

        if(playerData.health > playerData.MaxHealth()) playerData.health = playerData.MaxHealth();
        playerScript.GainStamina(playerData.MaxStamina());
        GlobalEvents.instance.PlayerStatsChange();

        Destroy (gameObject);
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

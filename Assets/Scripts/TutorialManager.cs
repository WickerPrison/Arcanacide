using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    InputManager im;
    SoundManager sm;
    [System.NonSerialized] public GameObject currentMessage;
    [System.NonSerialized] public GameObject nextMessage;
    bool messageOpen = false;
    public GameObject textsTutorial;
    string texts = "Texts";
    [SerializeField] GameObject attackTutorial;
    string attack = "Attack";
    [SerializeField] GameObject healTutorial;
    string heal = "Heal";
    [SerializeField] GameObject dodgeTutorial;
    string dodge = "Dodge";
    [SerializeField] GameObject swordSiteTutorial;
    string swordSite = "Sword Site";
    [SerializeField] GameObject swordSiteTutorial2;
    string swordSite2 = "Sword Site 2";
    [SerializeField] GameObject remnantTutorial;
    string remnant = "Remnant";
    [SerializeField] GameObject emblemTutorial;
    string emblem = "Emblem";
    [SerializeField] GameObject blockTutorial1;
    [SerializeField] GameObject blockTutorial2;
    string block = "Block";
    [SerializeField] GameObject brokenGemTutorial;
    string broken_gem = "Broken Gem";
    [SerializeField] GameObject specialAttackTutorial;
    string specialAttack = "Special Attack";
    [SerializeField] GameObject morePatchesTutorial;
    [SerializeField] GameObject endOfDemoTutorial;
    string endOfDemo = "End Of Demo";
    [SerializeField] GameObject altarTutorial;
    string altar = "Altar";
    [SerializeField] GameObject newWeaponTutorial;
    string newWeapon = "New Weapon";
    [SerializeField] GameObject mapTutorial;
    string map = "Map";
    public List<string> allTutorials;
    Dictionary<string, GameObject> tutorialDict;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        sm = im.gameObject.GetComponent<SoundManager>();
        im.controls.Tutorial.Select.performed += ctx => NextMessage();
        SetupTutorialDictionary();
        TutorialList();
    }

    void NextMessage()
    {
        if (!messageOpen)
        {
            return;
        }

        sm.ButtonSound();

        if (nextMessage != null)
        {
            Destroy(currentMessage);
            currentMessage = Instantiate(nextMessage);
            nextMessage = null;
        }
        else
        {
            CloseMessage();
        }
    }

    public void OpenMessage()
    {
        im.Tutorial();
        messageOpen = true;
        Time.timeScale = 0;
    }

    void CloseMessage()
    {
        im.Gameplay();
        messageOpen = false;
        Time.timeScale = 1;
        Destroy(currentMessage);
    }

    public void Tutorial(string tutorialName, string nextMessageName = "")
    {
        playerData.tutorials.Remove(tutorialName);
        nextMessage = tutorialDict[nextMessageName];
        currentMessage = Instantiate(tutorialDict[tutorialName]);
        OpenMessage();
    }

    public void NewAbilityTutorial(string ability)
    {
        playerData.tutorials.Remove(ability);
        switch (ability)
        {
            case "Block":
                nextMessage = blockTutorial2;
                currentMessage = Instantiate(blockTutorial1);
                break;
            case "Special Attack":
                nextMessage = null;
                currentMessage = Instantiate(specialAttackTutorial);
                break;
        }
        OpenMessage();
    }

    public void MorePatchesTutorial()
    {
        nextMessage = null;
        currentMessage = Instantiate(morePatchesTutorial);
        OpenMessage();
    }

    void TutorialList()
    {
        allTutorials.Clear();
        allTutorials.Add(texts);
        allTutorials.Add(attack);
        allTutorials.Add(heal);
        allTutorials.Add(broken_gem);
        allTutorials.Add(dodge);
        allTutorials.Add(swordSite);
        allTutorials.Add(swordSite2);
        allTutorials.Add(remnant);
        allTutorials.Add(emblem);
        allTutorials.Add(block);
        allTutorials.Add(specialAttack);
        allTutorials.Add(endOfDemo);
        allTutorials.Add(altar);
        allTutorials.Add(newWeapon);
        allTutorials.Add(map);
    }

    void SetupTutorialDictionary()
    {
        tutorialDict = new Dictionary<string, GameObject>
        {
            {"", null },
            {attack, attackTutorial },
            {heal, healTutorial },
            {broken_gem, brokenGemTutorial },
            {dodge, dodgeTutorial },
            {swordSite, swordSiteTutorial },
            {swordSite2, swordSiteTutorial2 },
            {remnant, remnantTutorial },
            {emblem, emblemTutorial },
            {endOfDemo, endOfDemoTutorial },
            {altar, altarTutorial},
            {newWeapon, newWeaponTutorial},
            {map, mapTutorial}
        };
    }
}

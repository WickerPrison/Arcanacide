using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    InputManager im;
    GameObject currentMessage;
    GameObject nextMessage;
    bool messageOpen = false;
    bool inGame = false;
    [SerializeField] GameObject walkTutorial;
    string walk = "Walk";
    [SerializeField] GameObject attackTutorial;
    string attack = "Attack";
    [SerializeField] GameObject healTutorial;
    string heal = "Heal";
    [SerializeField] GameObject dodgeTutorial;
    string dodge = "Dodge";
    [SerializeField] GameObject swordSiteTutorial;
    [SerializeField] GameObject swordSiteTutorial2;
    string swordSite = "Sword Site";
    [SerializeField] GameObject remnantTutorial;
    string remnant = "Remnant";
    [SerializeField] GameObject emblemTutorial;
    string emblem = "Emblem";
    [SerializeField] GameObject blockTutorial1;
    [SerializeField] GameObject blockTutorial2;
    string block = "Block";
    [SerializeField] GameObject endOfDemoTutorial;
    string endOfDemo = "EndOfDemo";
    [SerializeField] GameObject altarTutorial;
    string altar = "Altar";
    public List<string> allTutorials;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Tutorial.Select.performed += ctx => NextMessage();
        TutorialList();
        if(GameObject.FindGameObjectsWithTag("Player").Length > 0)
        {
            inGame = true;
        }
        else
        {
            inGame = false;
        }
    }

    private void Update()
    {
        if (inGame && playerData.tutorials.Contains(walk))
        {
            playerData.tutorials.Remove(walk);
            nextMessage = null;
            currentMessage = Instantiate(walkTutorial);
            OpenMessage();
        }
    }

    void NextMessage()
    {
        if (!messageOpen)
        {
            return;
        }

        if(nextMessage != null)
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

    void OpenMessage()
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

    public void AttackTutorial()
    {
        playerData.tutorials.Remove(attack);
        nextMessage = null;
        currentMessage = Instantiate(attackTutorial);
        OpenMessage();
    }

    public void HealTutorial()
    {
        playerData.tutorials.Remove(heal);
        nextMessage = null;
        currentMessage = Instantiate(healTutorial);
        OpenMessage();
    }

    public void DodgeTutorial()
    {
        playerData.tutorials.Remove(dodge);
        nextMessage = null;
        currentMessage = Instantiate(dodgeTutorial);
        OpenMessage();
    }

    public void SwordSiteTutorial()
    {
        playerData.tutorials.Remove(swordSite);
        nextMessage = swordSiteTutorial2;
        currentMessage = Instantiate(swordSiteTutorial);
        OpenMessage();
    }

    public void RemnantTutorial()
    {
        playerData.tutorials.Remove("Remnant");
        nextMessage = null;
        currentMessage = Instantiate(remnantTutorial);
        OpenMessage();
    }

    public void EmblemTutorial()
    {
        playerData.tutorials.Remove("Emblem");
        nextMessage = null;
        currentMessage = Instantiate(emblemTutorial);
        OpenMessage();
    }

    public void BlockTutorial()
    {
        playerData.tutorials.Remove(block);
        nextMessage = blockTutorial2;
        currentMessage = Instantiate(blockTutorial1);
        OpenMessage();
    }

    public void EndOfDemoTutorial()
    {
        playerData.tutorials.Remove("EndOfDemo");
        nextMessage = null;
        currentMessage = Instantiate(endOfDemoTutorial);
        OpenMessage();
    }

    public void AltarTutorial()
    {
        playerData.tutorials.Remove("Altar");
        nextMessage = null;
        currentMessage = Instantiate(altarTutorial);
        OpenMessage();
    }

    void TutorialList()
    {
        allTutorials.Clear();
        allTutorials.Add(walk);
        allTutorials.Add(attack);
        allTutorials.Add(heal);
        allTutorials.Add(dodge);
        allTutorials.Add(swordSite);
        allTutorials.Add(remnant);
        allTutorials.Add(emblem);
        allTutorials.Add(block);
        allTutorials.Add(endOfDemo);
        allTutorials.Add(altar);
    }
}

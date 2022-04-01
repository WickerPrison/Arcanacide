using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] string tutorialName;
    [SerializeField] PlayerData playerData;
    TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tutorialName)
            {
                case "Attack":
                    if (playerData.tutorials.Contains("Attack"))
                    {
                        tutorialManager.AttackTutorial();
                    }
                    break;
                case "Dodge":
                    if (playerData.tutorials.Contains("Dodge"))
                    {
                        tutorialManager.DodgeTutorial();
                    }
                    break;
                case "Sword Site":
                    if(playerData.tutorials.Contains("Sword Site"))
                    {
                        tutorialManager.SwordSiteTutorial();
                    }
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tutorialName)
            {
                case "Heal":
                    GameManager gm = tutorialManager.gameObject.GetComponent<GameManager>();
                    if (playerData.tutorials.Contains("Heal") && gm.enemies.Count <= 0)
                    {
                        tutorialManager.HealTutorial();
                    }
                    break;
            }
        }
    }
}

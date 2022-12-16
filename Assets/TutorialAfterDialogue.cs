using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialAfterDialogue : EndDialogueEvent
{
    [SerializeField] PlayerData playerData;
    TutorialManager tm;
    [SerializeField] string tutorialName;

    private void Start()
    {
        tm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TutorialManager>();
    }

    public override void EndEvent()
    {
        base.EndEvent();
        switch (tutorialName)
        {
            case "Attack":
                if (playerData.tutorials.Contains("Attack"))
                {
                    tm.AttackTutorial();
                }
                break;
            case "Texts":
                if (playerData.tutorials.Contains("Texts"))
                {
                    tm.TextsTutorial();
                }
                break;
        }
    }
}

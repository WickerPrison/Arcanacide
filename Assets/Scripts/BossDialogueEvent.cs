using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDialogueEvent : EndDialogueEvent
{
    BossController bossController;

    private void Start()
    {
        bossController = GetComponentInParent<BossController>();
    }

    public override void EndEvent()
    {
        base.EndEvent();
        bossController.hasSeenPlayer = true;
    }
}

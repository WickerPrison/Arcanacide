using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossDialogueEvent : MonoBehaviour, IEndDialogue
{
    BossController bossController;

    private void Start()
    {
        bossController = GetComponentInParent<BossController>();
    }

    public void EndDialogue()
    {
        bossController.state = EnemyState.IDLE;
    }
}

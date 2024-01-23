using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinalDialogue : Dialogue
{
    ChaosBossController bossController;

    public override void Start()
    {
        base.Start();
        bossController = GetComponent<ChaosBossController>();
    }

    public override void EndDialogue()
    {
        Debug.Log("test");
        GetComponentInChildren<ChaosBossAnimationEvenets>().StartDissolve();
    }
}

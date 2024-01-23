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
        GetComponentInChildren<ChaosBossAnimationEvenets>().StartDissolve();
        StartCoroutine(DissolveScreen());
    }

    IEnumerator DissolveScreen()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShader>().StartDissolve();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinalBossDialogue : Dialogue 
{
    FinalBossEvents bossEvents;

    public override void Start()
    {
        base.Start();
        bossEvents = GetComponentInParent<FinalBossEvents>();
    }

    public override void NextLine()
    {
        base.NextLine();

        if(currentLineIndex == 2)
        {
            bossEvents.StandUp();
        }
        else if(currentLineIndex == 3)
        {
            bossEvents.SitUp();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeffController : LightningThrower
{
    Dialogue dialogue;

    public override void Start()
    {
        base.Start();
        dialogue = GetComponent<Dialogue>();
        dialogue.SetConversation(0);
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(() => GlobalEvents.instance.EnableEnemies(true));
    }

    public override void StartDying()
    {
        SpecialAbilityOff();
        enemyEvents.StartDying();
        frontAnimator.Play("JeffStartDeath");
        backAnimator.Play("JeffStartDeath");
        dialogue.SetConversation(1); 
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(JeffDeath);
    }

    void JeffDeath()
    {
        frontAnimator.Play("JeffDeath");
        backAnimator.Play("JeffDeath");
        GlobalEvents.instance.EnableEnemies(true);
    }
}

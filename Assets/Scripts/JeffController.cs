using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JeffController : LightningThrower
{
    Dialogue dialogue;
    [SerializeField] MapData mapData;

    public override void Start()
    {
        if (mapData.carolsDeadFriends.Contains("Jeff"))
        {
            Destroy(gameObject);
            return;
        }

        base.Start();
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
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
        mapData.carolsDeadFriends.Add("Jeff");
        GlobalEvents.instance.EnableEnemies(true);
    }
}

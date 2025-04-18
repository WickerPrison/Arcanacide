using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaroldController : OldManController
{
    Dialogue dialogue;
    [SerializeField] MapData mapData;

    public override void Start()
    {
        if (mapData.carolsDeadFriends.Contains("Harold"))
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
        dialogue.SetConversation(2);
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(() => GlobalEvents.instance.EnableEnemies(true));
    }

    public override void StartDying()
    {
        staticVFX.Stop();
        enemyEvents.StartDying();
        frontAnimator.Play("HaroldStartDeath");
        backAnimator.Play("HaroldStartDeath");
        dialogue.SetConversation(3);
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(HaroldDeath);
    }

    void HaroldDeath()
    {
        frontAnimator.Play("HaroldDeath");
        backAnimator.Play("HaroldDeath");
        mapData.carolsDeadFriends.Add("Harold");
        GlobalEvents.instance.EnableEnemies(true);
    }
}

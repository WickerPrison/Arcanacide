using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArnoldController : ElectricSwordsmanController
{
    Dialogue dialogue;
    [SerializeField] MapData mapData;


    public override void Awake()
    {
        base.Awake();
        if (mapData.carolsDeadFriends.Contains("Arnold"))
        {
            Destroy(gameObject);
            return;
        }
    }

    public override void Start()
    {
        dialogue = GetComponent<Dialogue>();
        base.Start();
    }

    public override void StartDying()
    {
        enemyEvents.StartDying();
        frontAnimator.Play("ArnoldStartDeath");
        backAnimator.Play("ArnoldStartDeath");
        dialogue.SetConversation(4);
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(HaroldDeath);
    }

    void HaroldDeath()
    {
        frontAnimator.Play("ArnoldDeath");
        backAnimator.Play("ArnoldDeath");
        mapData.carolsDeadFriends.Add("Arnold");
        GlobalEvents.instance.EnableEnemies(true);
    }
}

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
        dialogue = GetComponent<Dialogue>();
        dialogue.SetConversation(2);
        GlobalEvents.instance.EnableEnemies(false);
        dialogue.StartWithCallback(() => GlobalEvents.instance.EnableEnemies(true));
    }
}

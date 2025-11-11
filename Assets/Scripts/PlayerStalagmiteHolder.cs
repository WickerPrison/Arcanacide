using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStalagmiteHolder : StalagmiteHolder
{
    PlayerAbilities playerAbilities;
    [SerializeField] AttackProfiles attackProfile;

    public override void Awake()
    {
        playerAbilities = GetComponentInParent<PlayerAbilities>();
    }

    public override void Start()
    {
        base.Start();
        foreach(StalagmiteAttack stalagmite in stalagmites)
        {
            stalagmite.attackProfile = attackProfile;
            stalagmite.playerAbilities = playerAbilities;
        }
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        
    }
}

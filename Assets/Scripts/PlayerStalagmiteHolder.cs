using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStalagmiteHolder : StalagmiteHolder
{
    PlayerAbilities playerAbilities;
    [SerializeField] AttackProfiles attackProfile;
    Quaternion startingRotation;
    Vector3 startingPosition;
    Transform attackPoint;

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

        attackPoint = transform.parent;
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
    }

    public override void TriggerWave()
    {
        base.CancelWave();
        transform.SetParent(attackPoint);
        transform.localRotation = startingRotation;
        transform.localPosition = startingPosition;
        transform.parent = null;
        base.TriggerWave();
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        
    }
}

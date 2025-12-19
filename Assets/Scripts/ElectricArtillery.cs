using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricArtillery : PlayerProjectile
{
    Vector3 targetPoint;
    GameManager gm;
    [SerializeField] float homingRange;
    PlayerFireballAnimations animations;
    [SerializeField] float arcHeight;
    [SerializeField] float thirdPointX;
    [SerializeField] float timeToHit;
    ArcUtils.ArcData arcData;
    enum ElectricArtilleryState
    {
        SPAWNED, ARC, HOMING
    }
    ElectricArtilleryState state;
    bool instantiatedCorrectly = false;

    public static ElectricArtillery Instantiate(GameObject prefab, Vector3 spawnOrigin, Vector3 direction, PlayerFireballAnimations animations, AttackProfiles attackProfile)
    {
        ElectricArtillery electricArtillery = Instantiate(prefab).GetComponent<ElectricArtillery>();
        float spawnSpread = 1f;
        float xOffset = Random.Range(-spawnSpread, spawnSpread);
        float zOffset = Random.Range(-spawnSpread, spawnSpread);
        float yOffset = Random.Range(0.1f, 1f);
        electricArtillery.transform.position = spawnOrigin + new Vector3(xOffset, yOffset, zOffset);
        electricArtillery.state = ElectricArtilleryState.SPAWNED;

        float targetSpread = 8f;
        xOffset = Random.Range(-targetSpread, targetSpread);
        zOffset = Random.Range(-targetSpread, targetSpread);
        float distance = Random.Range(10f, 15f);
        electricArtillery.targetPoint = spawnOrigin + new Vector3(xOffset, 0, zOffset) + direction * distance;

        electricArtillery.animations = animations;
        electricArtillery.ListenForEvent();

        electricArtillery.attackProfile = attackProfile;

        electricArtillery.instantiatedCorrectly = true;
        return electricArtillery;
    }

    private void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("ElectricArtillery");
        arcData = ArcUtils.CreateArcData(transform.position, targetPoint, timeToHit, arcHeight, thirdPointX);
        if (arcData.speed < speed) arcData.speed = speed;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public override void FixedUpdate()
    {
        switch (state)
        {
            case ElectricArtilleryState.SPAWNED:
                Hover();
                break;
            case ElectricArtilleryState.ARC:
                Arc();
                break;
            case ElectricArtilleryState.HOMING:
                Homing();
                break;
        }
    }

    void Hover()
    {

    }

    void Arc()
    {
        transform.position = ArcUtils.GetNextArcPosition(transform.position, arcData);
        GetTargetEnemy();
        if(target != null)
        {
            state = ElectricArtilleryState.HOMING;
        }
        if(transform.position.y <= 0)
        {
            KillProjectile();
        }
    }

    void Homing()
    {
        GetTargetEnemy();
        if (target == null) KillProjectile();
        Vector3 direction = target.position + Vector3.up * 1.5f - transform.position;
        transform.position += direction * Time.fixedDeltaTime * speed;
    }

    void GetTargetEnemy()
    {
        target = null;
        float distance = 100f;
        foreach(EnemyScript enemy in gm.enemies)
        {
            float distToEnemy = Vector3.Distance(Vector3.Scale(transform.position, new Vector3(1, 0, 1)), enemy.transform.position);
            if(distToEnemy < distance && distToEnemy < homingRange)
            {
                target = enemy.transform;
            }
        }
    }

    void ListenForEvent()
    {
        animations.onLaunchFireWave += Animations_onLaunchFireWave;
    }

    private void OnDisable()
    {
        animations.onLaunchFireWave -= Animations_onLaunchFireWave;
    }

    private void Animations_onLaunchFireWave(object sender, System.EventArgs e)
    {
        state = ElectricArtilleryState.ARC;
    }
}

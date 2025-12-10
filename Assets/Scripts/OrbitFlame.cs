using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFlame : PlayerProjectile
{
    bool instantiatedCorrectly = false;
    float offsetAngle;
    float radius;
    PlayerScript playerScript;
    Action<OrbitFlame> deathCallback;
    ParticleSystem particles;

    public static OrbitFlame Instantiate(GameObject prefab, float offsetAngle, float radius, PlayerScript playerScript, Action<OrbitFlame> deathCallback, AttackProfiles attackProfile)
    {
        OrbitFlame flame = Instantiate(prefab).GetComponent<OrbitFlame>();
        flame.offsetAngle = offsetAngle;
        flame.radius = radius;
        flame.playerScript = playerScript;
        flame.deathCallback = deathCallback;
        flame.attackProfile = attackProfile;
        flame.instantiatedCorrectly = true;
        return flame;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("OrbitFlame");
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
       
    }

    public void PositionFlame(float angle)
    {
        Vector3 direction = Utils.RotateDirection(Vector3.right, angle + offsetAngle) * radius;
        transform.position = direction + Vector3.up * 1.5f + playerScript.transform.position;
        if (!particles.isPlaying)
        {
            particles.Play();
        }
    }

    public override void HitObject(Collider collision)
    {
        //do nothing
    }

    public override void KillProjectile()
    {
        deathCallback(this);
        base.KillProjectile();
    }
}

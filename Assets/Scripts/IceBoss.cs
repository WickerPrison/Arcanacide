using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class IceBoss : EnemyController
{
    [SerializeField] GameObject iciclePrefab;
    IceBossBeamCrystal beamCrystal;

    float playerDistance;
    float tooFarAway = 5;

    float icicleTimer;
    float icicleMaxTime = 1;

    LayerMask layerMask;
    [SerializeField] LineRenderer line;
    Vector3 offset = new Vector3(0, 2, 0);
    Gradient gradient;
    [SerializeField] Color blueColor;
    float alpha = 1;
    float aimValue;
    float maxAimValue = 3;
    float gradientOffset = .4f;
    Vector3 away = new Vector3(100, 100, 100);

    public override void Start()
    {
        base.Start();

        beamCrystal = GetComponentInChildren<IceBossBeamCrystal>();

        line.SetPosition(0, away);
        line.SetPosition(1, away);

        layerMask = LayerMask.GetMask("Default", "Player", "IFrames");

        gradient = new Gradient();
        GradientColorKey[] colorKey = { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(blueColor, 1.0f) };
        GradientAlphaKey[] alphaKey = { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) };
        gradient.SetKeys(colorKey, alphaKey);

        aimValue = maxAimValue;
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        if (hasSeenPlayer)
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            playerDistance = Vector3.Distance(transform.position, playerController.transform.position);

            if(playerDistance > tooFarAway)
            {
                RainIcicles();

                if (!beamCrystal.spawnedIn)
                {
                    beamCrystal.SpawnIn();
                    line.SetPosition(0, away);
                    line.SetPosition(1, away);
                }
                else
                {
                    ShowBeam();
                }
            }
            else
            {
                if (beamCrystal.spawnedIn)
                {
                    beamCrystal.SpawnOut();
                }
                line.SetPosition(0, away);
                line.SetPosition(1, away);
            }
        }

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
    }

    private void RainIcicles()
    {
        icicleTimer -= Time.deltaTime;
        if(icicleTimer < 0)
        {
            icicleTimer = icicleMaxTime;
            Transform icicle = Instantiate(iciclePrefab).transform;
            icicle.position = playerController.transform.position;
        }
    }

    void ShowBeam()
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, playerScript.transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore);
        line.SetPosition(0, beamCrystal.transform.position);

        if (hit.collider.CompareTag("Player"))
        {
            line.SetPosition(1, playerScript.transform.position + offset);
            aimValue -= Time.deltaTime;
            if (aimValue < -gradientOffset)
            {
                aimValue = maxAimValue;
                Shoot();
            }
        }
        else
        {
            line.SetPosition(1, hit.point + offset);
            aimValue = maxAimValue;
        }

        float aimRatio = aimValue / maxAimValue;

        if (aimRatio < 1 - gradientOffset)
        {
            line.startColor = gradient.Evaluate(aimRatio + gradientOffset);
        }
        else
        {
            line.startColor = blueColor;
        }
        line.endColor = gradient.Evaluate(aimRatio);
    }

    void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = beamCrystal.transform.position;
        projectile.direction = playerController.transform.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, projectile.direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
    }
}

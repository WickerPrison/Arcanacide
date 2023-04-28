using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceBeamController : EnemyController
{
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
    int strafeLeftOrRight = -1;
    float strafeSpeed = 0.5f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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

        if (state == EnemyState.UNAWARE)
        {
            RaycastHit hit;
            Physics.Linecast(transform.position, playerScript.transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore);
            if (hit.collider.CompareTag("Player"))
            {
                state = EnemyState.IDLE;
                gm.awareEnemies += 1;
            }
        }
        else
        {
            //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
            if (navAgent.enabled == true)
            {
                navAgent.SetDestination(playerController.transform.position);
            }

            ShowBeam();
        }
    }

    void ShowBeam()
    {
        RaycastHit hit;
        Physics.Linecast(transform.position, playerScript.transform.position, out hit, layerMask, QueryTriggerInteraction.Ignore);
        line.SetPosition(0, transform.position + offset);

        if (hit.collider.CompareTag("Player"))
        {
            line.SetPosition(1, playerScript.transform.position + offset);
            aimValue -= Time.deltaTime; 
            strafeLeftOrRight *= -1;
            if(aimValue < -gradientOffset)
            {
                aimValue = maxAimValue;
                Shoot();
            }
        }
        else
        {
            line.SetPosition(1, hit.point + offset);
            aimValue = maxAimValue;
            Strafe();
        }

        float aimRatio = aimValue / maxAimValue;

        if(aimRatio < 1 - gradientOffset)
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
        projectile.transform.position = transform.position + offset;
        projectile.direction = playerController.transform.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, projectile.direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
    }

    void Strafe()
    {
        Vector3 playerToEnemy = transform.position - playerController.transform.position;
        playerToEnemy *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(Vector3.up, playerToEnemy.normalized);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }

    public override void StartStagger(float staggerDuration)
    {
        
    }
}

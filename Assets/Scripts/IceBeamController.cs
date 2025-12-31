using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class IceBeamController : EnemyController
{
    [SerializeField] BeamVfx beamVfx;
    Vector3 offset = new Vector3(0, 1.8f, 0);
    int strafeLeftOrRight = -1;
    float strafeSpeed = 0.5f;
    Vector3 initialPosition;
    [SerializeField] Transform crystal;
    float crystalStartPos;
    float crystalFloatPos;
    float crystalFloatDir = 1;
    [SerializeField] float crystalFloatRate;
    [SerializeField] float crystalFloatAmp;
    [SerializeField] bool changeColor = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        beamVfx.HideBeam();
        crystalStartPos = crystal.localPosition.y;
    }

    public override void Update()
    {
        base.Update();
        Float();
    }

    public override void EnemyAI()
    {
        base.EnemyAI();

        switch (state)
        {
            case EnemyState.IDLE:
                //navAgent is the pathfinding component. It will be enabled whenever the enemy is allowed to walk
                if (navAgent.enabled == true)
                {
                    navAgent.SetDestination(playerScript.transform.position);
                }

                ShowBeam();
                break;
            case EnemyState.DISABLED:
                beamVfx.HideBeam();
                break;
        }
    }

    void ShowBeam()
    {
        RaycastHit hit = beamVfx.BeamHitscan(transform.position, playerScript.transform.position);
        initialPosition = transform.position + Vector3.up * 1.45f + Vector3.up * crystalFloatPos;
        initialPosition += (playerScript.transform.position - transform.position).normalized * .6f;

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            beamVfx.SetPositions(initialPosition, playerScript.transform.position + offset);
            beamVfx.DecrementAimValue(Shoot);
            strafeLeftOrRight *= -1;
        }
        else
        {
            beamVfx.SetPositions(initialPosition, hit.point + offset);
            beamVfx.ResetAimValue();
            Strafe();
        }
    }

    IEnumerator ShotClock()
    {
        state = EnemyState.SPECIAL;
        yield return new WaitForSeconds(1);
        state = EnemyState.IDLE;
    }

    void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position + offset;
        projectile.direction = playerScript.transform.position - transform.position;
        float angle = Vector3.SignedAngle(Vector3.forward, projectile.direction, Vector3.up);
        projectile.transform.rotation = Quaternion.Euler(25, 0, -angle);
        if (changeColor)
        {
            SpriteColorChange spriteColorChange = projectile.GetComponent<SpriteColorChange>();
            spriteColorChange.colorChange = true;
            ParticleColorChange particleColorChange = projectile.GetComponent<ParticleColorChange>();
            particleColorChange.colorChange = true;
        }
        beamVfx.HideBeam();
        StartCoroutine(ShotClock());
    }

    void Strafe()
    {
        Vector3 playerToEnemy = transform.position - playerScript.transform.position;
        playerToEnemy *= strafeLeftOrRight;
        Vector3 strafeDirection = Vector3.Cross(Vector3.up, playerToEnemy.normalized);
        navAgent.Move(strafeDirection.normalized * Time.deltaTime * strafeSpeed);
    }

    void Float()
    {
        float thing = (crystalFloatAmp - Mathf.Abs(crystalFloatPos)) / crystalFloatAmp;
        float distanceTraveled = crystalFloatRate * Time.deltaTime * thing;
        crystalFloatPos += distanceTraveled * crystalFloatDir;
        if (crystalFloatDir == 1 && crystalFloatPos > crystalFloatAmp * 8/10)
        {
            crystalFloatDir = -1;
        }
        else if(crystalFloatDir == -1 && crystalFloatPos < -crystalFloatAmp * 8/10)
        {
            crystalFloatDir = 1;
        }
        crystal.localPosition = new Vector3(0, crystalFloatPos + crystalStartPos, 0);
    }

    public override void StartDying()
    {
        beamVfx.HideBeam();
        base.StartDying();
    }

    public override void StartStagger(float staggerDuration)
    {
        
    }
}

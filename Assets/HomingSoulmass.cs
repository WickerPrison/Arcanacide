using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Timeline;

public class HomingSoulmass : MonoBehaviour
{
    private enum HomingSoulmassState
    {
        ORBIT, PROJECTILE
    }

    Vector3 center;
    [SerializeField] float angle = 0;
    [SerializeField] AudioClip playerImpactSFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] float impactSFXvolume;
    float speed = 10;
    float rotationSpeed = 100;
    int spellDamage = 10;
    float poiseDamage = 10;
    float range = 4;
    float playerDistance;
    float turnAngle = 25f;
    PlayerScript playerScript;
    EnemyScript enemyOfOrigin;
    Vector3 attackOffset = Vector3.up;
    HomingSoulmassState state = HomingSoulmassState.ORBIT;

    private void Start()
    {
        enemyOfOrigin = GetComponentInParent<EnemyScript>();
        playerScript = enemyOfOrigin.GetComponent<EnemyController>().playerScript;
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);
        if(state == HomingSoulmassState.ORBIT)
        {
            if(playerDistance <= range)
            {
                transform.LookAt(playerScript.transform.position + attackOffset);
                state = HomingSoulmassState.PROJECTILE;
            }
            else
            {
                UpdatePosition();
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == HomingSoulmassState.ORBIT) return;

        Vector3 targetPosition = playerScript.transform.position + attackOffset;
        Vector3 rayDirection = transform.position - targetPosition;
        float angleToTarget = Mathf.Acos(Vector3.Dot(-rayDirection, transform.forward) / (rayDirection.magnitude * transform.forward.magnitude));
        angleToTarget *= Mathf.Rad2Deg;
        if (angleToTarget <= turnAngle * Time.fixedDeltaTime)
        {
            transform.LookAt(targetPosition);
        }
        else
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, turnAngle * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }

        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    void UpdatePosition()
    {
        center = enemyOfOrigin.transform.position + attackOffset;
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360) angle -= 360;

        transform.position = center + RotateDirection(Vector3.right, angle).normalized * 2;
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Collision(collision);
    }

    public void Collision(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.layer == 3)
            {
                HitPlayer(collision);
            }
            else if (collision.gameObject.layer == 8)
            {
                PerfectDodge(collision);
            }
        }
        else
        {
            HitObject(collision);
        }
    }

    public virtual void HitPlayer(Collider collision)
    {
        playerScript = collision.gameObject.GetComponent<PlayerScript>();
        playerScript.LoseHealth(spellDamage, enemyOfOrigin);
        playerScript.LosePoise(poiseDamage);
        AudioSource.PlayClipAtPoint(playerImpactSFX, transform.position, impactSFXvolume);
        Destroy(gameObject);
    }

    public virtual void PerfectDodge(Collider collision)
    {
        PlayerController playerController;
        playerController = collision.gameObject.GetComponent<PlayerController>();
        playerController.PerfectDodge();
    }

    public virtual void HitObject(Collider collision)
    {
        if(state == HomingSoulmassState.PROJECTILE)
        {
            AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
            Destroy(gameObject);
        }
    }
}

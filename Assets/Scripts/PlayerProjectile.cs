using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerProjectile : MonoBehaviour
{
    public int speed;
    public PlayerController playerController;
    [SerializeField] AudioClip enemyImpactSFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] float impactSFXvolume;
    [SerializeField] float lifetime;
    public Transform target;
    public float turnAngle;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HitEnemy(collision);
        }
        else
        {
            HitObject(collision);
        }
    }

    public virtual void HitEnemy(Collider collision)
    {
        EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
        enemyScript.LoseHealth(playerController.AttackPower() * 2, playerController.AttackPower() * 2);
        //AudioSource.PlayClipAtPoint(enemyImpactSFX, transform.position, impactSFXvolume);
        Destroy(gameObject);
    }

    public virtual void HitObject(Collider collision)
    {
        AudioSource.PlayClipAtPoint(impactSFX, transform.position, impactSFXvolume);
        Destroy(gameObject);
    }

    public virtual void FixedUpdate()
    {
        Vector3 rayDirection = transform.position - target.position;
        float angleToTarget = Mathf.Acos(Vector3.Dot(-rayDirection, transform.forward) / (rayDirection.magnitude * transform.forward.magnitude));
        angleToTarget *= Mathf.Rad2Deg;
        if (angleToTarget <= turnAngle * Time.fixedDeltaTime)
        {
            transform.LookAt(target);
        }
        else
        {
            Vector3 rotateDirection = Vector3.RotateTowards(transform.forward, target.position - transform.position, turnAngle * Mathf.Deg2Rad * Time.fixedDeltaTime, 0);
            transform.rotation = Quaternion.LookRotation(rotateDirection);
        }

        transform.position += transform.forward * Time.fixedDeltaTime * speed;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}

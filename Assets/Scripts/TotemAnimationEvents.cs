using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemAnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] ParticleSystem landingVFX;
    CameraFollow cameraScript;
    TouchingCollider colliderScript;
    List<Collider> touchingCollider;
    PlayerController playerController;
    int damageMultiplier = 2;
    float startRadius = 2;
    int numberOfBoxes = 50;
    float rippleSpeed = 5;
    float lifeTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        colliderScript = GetComponentInParent<TouchingCollider>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Landing()
    {
        landingVFX.Play();
        StartCoroutine(cameraScript.ScreenShake(.1f, .03f));
        touchingCollider = colliderScript.GetTouchingObjects();
        foreach(Collider collider in touchingCollider)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyScript enemyScript = collider.gameObject.GetComponent<EnemyScript>();
                enemyScript.LoseHealth(playerController.AttackPower() * damageMultiplier, playerController.AttackPower() * damageMultiplier);
            }
            else if (collider.gameObject.CompareTag("Player"))
            {
                PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(playerController.AttackPower() * damageMultiplier);
                playerScript.LosePoise(playerController.AttackPower() * damageMultiplier);
            }
        }
    }

    public void Ripple()
    {
        float rotateAngle = 360 / numberOfBoxes;
        for(int box = 0; box < numberOfBoxes; box++)
        {
            RippleBox rippleBox = Instantiate(ripplePrefab).GetComponent<RippleBox>();
            rippleBox.transform.position = transform.position + new Vector3(startRadius, 0, 0);
            rippleBox.transform.RotateAround(transform.position, transform.up, rotateAngle * box);
            rippleBox.rippleSpeed = rippleSpeed;
            rippleBox.lifeTime = lifeTime;
            rippleBox.direction = Vector3.Normalize(rippleBox.transform.position - transform.position);
        }
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}

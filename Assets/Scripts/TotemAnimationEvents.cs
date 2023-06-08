 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemAnimationEvents : MonoBehaviour
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] ParticleSystem landingVFX;
    [SerializeField] AttackProfiles axeSpecial;
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [System.NonSerialized] public ExternalLanternFairy lanternFairy;
    [SerializeField] Transform fairySprite;
    CameraFollow cameraScript;
    TouchingCollider colliderScript;
    List<Collider> touchingCollider;
    float startRadius = 2;
    int numberOfBoxes = 50;
    float rippleSpeed = 5;
    float lifeTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        colliderScript = GetComponentInParent<TouchingCollider>();
    }

    public void Landing()
    {
        landingVFX.Play();
        StartCoroutine(cameraScript.ScreenShake(axeSpecial.screenShakeNoHit.x, axeSpecial.screenShakeNoHit.y));
        touchingCollider = colliderScript.GetTouchingObjects();
        int damage = Mathf.RoundToInt(playerData.AttackPower() * axeSpecial.damageMultiplier);
        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
        {
            damage += Mathf.RoundToInt(damage * emblemLibrary.arcaneMasteryPercent);
        }
        int poiseDamage = Mathf.RoundToInt(playerData.AttackPower() * axeSpecial.damageMultiplier);
        foreach(Collider collider in touchingCollider)
        {
            if(collider != null)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyScript enemyScript = collider.gameObject.GetComponent<EnemyScript>();
                    enemyScript.LoseHealth(damage, poiseDamage);
                    enemyScript.StartStagger(axeSpecial.staggerDuration);
                }
                else if (collider.gameObject.CompareTag("Player"))
                {
                    PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
                    playerScript.LoseHealth(damage);
                    playerScript.LosePoise(poiseDamage);
                    playerScript.StartStagger(axeSpecial.staggerDuration);
                }
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
            WaveBox waveBox = rippleBox.gameObject.GetComponent<WaveBox>();
            waveBox.damage = Mathf.RoundToInt(playerData.ArcaneDamage() * axeSpecial.magicDamageMultiplier);
            if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_mastery))
            {
                waveBox.damage += Mathf.RoundToInt(waveBox.damage * emblemLibrary.arcaneMasteryPercent);
            }
            waveBox.poiseDamage = Mathf.RoundToInt(playerData.ArcaneDamage() * axeSpecial.magicDamageMultiplier);
        }
    }

    public void SelfDestruct()
    {
        lanternFairy.Return(fairySprite.position);
        Destroy(gameObject);
    }
}

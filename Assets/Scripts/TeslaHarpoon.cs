using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaHarpoon : EnemyController
{
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject brokenSprite;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem particles;
    public Transform lightningOrigin;
    SpriteEffects spriteEffects;
    [System.NonSerialized] public HarpoonManager harpoonManager;

    public override void Start()
    {
        base.Start();
        spriteEffects = GetComponent<SpriteEffects>();
        harpoonManager.AddHarpoon(this);
    }

    public override void Update()
    {
        
    }

    public override void StartDying()
    {
        state = EnemyState.DYING;
        harpoonManager.RemoveHarpoon(this);
        sprite.SetActive(false);
        brokenSprite.SetActive(true);
        animator.Play("Destroy");
        particles.Play();
        StartCoroutine(DeathTimer());
    }

    public void DirecitonalAttack(Vector3 attackOrigin)
    {
        if(attackOrigin.x < transform.position.x)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(spriteEffects.Dissolve());
        yield return new WaitForSeconds(1f);
        enemyScript.Death();
    }

    public override void StartStagger(float staggerDuration) {}
}

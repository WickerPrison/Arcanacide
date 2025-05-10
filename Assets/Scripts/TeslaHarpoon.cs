using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaHarpoon : EnemyController
{
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject brokenSprite;
    SpriteEffects spriteEffects;

    public override void Start()
    {
        base.Start();
        spriteEffects = GetComponent<SpriteEffects>();
    }

    public override void Update()
    {
        
    }

    public override void StartDying()
    {
        state = EnemyState.DYING;
        sprite.SetActive(false);
        brokenSprite.SetActive(true);
        StartCoroutine(DeathTimer());
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

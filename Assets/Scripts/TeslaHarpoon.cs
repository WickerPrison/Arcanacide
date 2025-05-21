using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaHarpoon : EnemyController
{
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject brokenSprite;
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

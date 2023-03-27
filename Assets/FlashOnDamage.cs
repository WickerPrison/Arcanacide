using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour
{
    EnemyScript enemyScript;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material spriteMaterial;
    SpriteRenderer[] renderers;
    WaitForSecondsRealtime flashTime = new WaitForSecondsRealtime(.15f);
    bool isFlashing = false;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void StartFlash(object sender, System.EventArgs e)
    {
        if (!isFlashing)
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        isFlashing = true;
        foreach(SpriteRenderer sprite in renderers)
        {
            sprite.material = whiteMaterial;
        }
        yield return flashTime;
        foreach (SpriteRenderer sprite in renderers)
        {
            sprite.material = spriteMaterial;
        }
        isFlashing = false;
    }

    private void OnEnable()
    {
        enemyScript.OnTakeDamage += StartFlash;
    }

    private void OnDisable()
    {
        enemyScript.OnTakeDamage -= StartFlash;
    }
}

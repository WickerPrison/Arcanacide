using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{
    EnemyScript enemyScript;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material spriteMaterial;
    SpriteRenderer[] renderers;
    WaitForSecondsRealtime flashTime = new WaitForSecondsRealtime(.15f);
    WaitForSecondsRealtime delayTime = new WaitForSecondsRealtime(0.3f);
    bool isFlashing = false;
    bool isDelayed = false;
    float dissolveTime = 1f;
    float dissolveSpeed;
    float dissolveProg = 0;

    private void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void StartFlash(object sender, System.EventArgs e)
    {
        if (!isFlashing && ! isDelayed)
        {
            StartCoroutine(Delay());
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

    IEnumerator Delay()
    {
        isDelayed = true;
        yield return delayTime;
        isDelayed = false;
    }

    public IEnumerator Dissolve()
    {
        dissolveSpeed = .8f / dissolveTime;
        while (dissolveProg < 1)
        {
            dissolveProg += dissolveSpeed * Time.deltaTime;
            foreach(SpriteRenderer sprite in renderers)
            {
                sprite.material.SetFloat("_DissolveProg", dissolveProg);
            }
            yield return new WaitForEndOfFrame();
        }
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

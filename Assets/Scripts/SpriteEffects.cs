using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteEffects : MonoBehaviour
{
    EnemyEvents enemyEvents;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material spriteMaterial;
    SpriteRenderer[] allRenderers;
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    WaitForSecondsRealtime flashTime = new WaitForSecondsRealtime(.15f);
    WaitForSecondsRealtime delayTime = new WaitForSecondsRealtime(0.3f);
    bool isFlashing = false;
    bool isDelayed = false;
    float dissolveTime = 1f;
    float dissolveSpeed;
    float dissolveProg = 0;

    private void Awake()
    {
        enemyEvents = GetComponent<EnemyEvents>();
        allRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer renderer in allRenderers)
        {
            if(renderer.sharedMaterial == spriteMaterial)
            {
                renderers.Add(renderer);
            }
        }
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
        dissolveProg = 0;
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
        enemyEvents.OnTakeDamage += StartFlash;
    }

    private void OnDisable()
    {
        enemyEvents.OnTakeDamage -= StartFlash;
    }
}

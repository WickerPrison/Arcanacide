using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SpriteEffects : MonoBehaviour
{
    EnemyEvents enemyEvents;
    [SerializeField] Material spriteMaterial;
    SpriteRenderer[] allRenderers;
    List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    WaitForSecondsRealtime flashTime = new WaitForSecondsRealtime(.15f);
    WaitForSecondsRealtime delayTime = new WaitForSecondsRealtime(0.3f);
    bool isFlashing = false;
    bool isDelayed = false;
    float dissolveSpeed;
    float dissolveProg = 0;
    public bool colorChange = false;
    [SerializeField] float colorChangeThreshold = 0.1f;
    [SerializeField] Color originalColor;
    [SerializeField] LevelColor newFloorColor;
    Color newColor;
    [SerializeField] ColorData colorData;

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

        if (colorChange)
        {
            newColor = colorData.GetColor(newFloorColor);
            foreach(SpriteRenderer sprite in renderers)
            {
                sprite.material.SetFloat("_ColorChange", 1);
                sprite.material.SetColor("_OriginalColor", originalColor);
                sprite.material.SetColor("_NewColor", newColor);
                sprite.material.SetFloat("_ColorChangeThreshold", colorChangeThreshold);
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
            sprite.material.SetFloat("_FlashWhite", 1);
        }
        yield return flashTime;
        foreach (SpriteRenderer sprite in renderers)
        {
            sprite.material.SetFloat("_FlashWhite", 0);
        }
        isFlashing = false;
    }

    IEnumerator Delay()
    {
        isDelayed = true;
        yield return delayTime;
        isDelayed = false;
    }

    public IEnumerator Dissolve(float dissolveTime = 1f)
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
            yield return endOfFrame;
        }
    }

    public IEnumerator UnDissolve(float dissolveTime = 1f)
    {
        dissolveProg = .8f;
        dissolveSpeed = .8f / dissolveTime;
        while (dissolveProg > 0)
        {
            dissolveProg -= dissolveSpeed * Time.deltaTime;
            foreach (SpriteRenderer sprite in renderers)
            {
                sprite.material.SetFloat("_DissolveProg", dissolveProg);
            }
            yield return endOfFrame;
        }
    }

    public void SetDissolve(float dissolve)
    {
        foreach (SpriteRenderer sprite in renderers)
        {
            sprite.material.SetFloat("_DissolveProg", dissolve);
        }
    }

    private void OnEnable()
    {
        if(enemyEvents != null)
        {
            enemyEvents.OnTakeDamage += StartFlash;
        }
    }

    private void OnDisable()
    {
        if(enemyEvents != null)
        {
            enemyEvents.OnTakeDamage -= StartFlash;
        }
    }
}

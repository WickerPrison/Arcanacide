using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteEffects : MonoBehaviour
{
    [SerializeField] Material spriteMaterial;
    PlayerEvents playerEvents;
    WaitForSecondsRealtime flashTime = new WaitForSecondsRealtime(.15f);
    WaitForSecondsRealtime delayTime = new WaitForSecondsRealtime(0.3f);
    bool isFlashing;
    bool isDelayed;

    void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
        spriteMaterial.SetFloat("_FlashWhite", 0);
    }

    private void onTakeDamage(object sender, System.EventArgs e)
    {
        if (!isFlashing && !isDelayed)
        {
            StartCoroutine(Delay());
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        isFlashing = true;
        spriteMaterial.SetFloat("_FlashWhite", 1);
        yield return flashTime;
        spriteMaterial.SetFloat("_FlashWhite", 0);
        isFlashing = false;
    }

    IEnumerator Delay()
    {
        isDelayed = true;
        yield return delayTime;
        isDelayed = false;
    }

    private void OnEnable()
    {
        playerEvents.onTakeDamage += onTakeDamage;
    }

    private void OnDisable()
    {
        playerEvents.onTakeDamage -= onTakeDamage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlicker : MonoBehaviour
{
    float timer;
    float flickerNum;
    WaitForSeconds flickerTime = new WaitForSeconds(0.1f);
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        float rand = Random.Range(0, 2);
        if(rand == 0) spriteRenderer.enabled = false;
        SetFlicker();
    }

    void SetFlicker()
    {
        timer = Random.Range(1, 5);
        flickerNum = Random.Range(0, 5);
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        yield return new WaitForSeconds(timer);
        for(int i = 0; i < flickerNum; i++)
        {
            yield return flickerTime;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        SetFlicker();
    }
}

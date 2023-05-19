using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleDrip : MonoBehaviour
{
    [SerializeField] Vector2 dripShift;
    [SerializeField] Vector2 dripScale;
    float loopTime;
    SpriteRenderer spriteRenderer;
    float time = 0;

    private void Awake()
    {
        loopTime = Random.Range(1f, 4f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_XScale", dripScale.x);
        spriteRenderer.material.SetFloat("_YScale", dripScale.y);
        spriteRenderer.material.SetFloat("_XShift", dripShift.x);
        spriteRenderer.material.SetFloat("_YShift", dripShift.y);
    }

    private void Update()
    {
        spriteRenderer.material.SetFloat("_XScale", dripScale.x);
        spriteRenderer.material.SetFloat("_YScale", dripScale.y);
        spriteRenderer.material.SetFloat("_XShift", dripShift.x);
        spriteRenderer.material.SetFloat("_YShift", dripShift.y);



        time += Time.deltaTime;
        if(time > loopTime)
        {
            loopTime = Random.Range(1f, 4f);
            time = 0;
        }
        spriteRenderer.material.SetFloat("_MyTime", time);
    }
}

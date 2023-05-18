using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleDrip : MonoBehaviour
{
    [SerializeField] Material dripShader;
    [SerializeField] float loopTime;
    float dripRange = 0.5f;
    Vector2 dripShift;
    float time = 0;

    private void Start()
    {
        dripShift.x = Random.Range(-dripRange, dripRange);
        dripShift.y = Random.Range(-dripRange, dripRange);
        dripShader.SetFloat("_XScale", transform.localScale.x * transform.parent.localScale.x);
        dripShader.SetFloat("_YScale", transform.localScale.z * transform.parent.localScale.z);
        dripShader.SetFloat("_XShift", dripShift.x);
        dripShader.SetFloat("_YShift", dripShift.y);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time > loopTime)
        {
            time = 0;
        }
        dripShader.SetFloat("_MyTime", time);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantAC : ArcProjectile
{
    [SerializeField] GameObject ripplePrefab;
    [SerializeField] Transform sprite;
    bool atDestination = false;
    [SerializeField] float initialDelay;
    [SerializeField] int rippleNum;
    [SerializeField] float rippleDelay;

    public override void Explosion()
    {
        atDestination = true;
        transform.position = endPoint;
        sprite.transform.localRotation = Quaternion.Euler(new Vector3(0,0,90));
        StartCoroutine(Ripples());
    }

    public override void SpawnIndicator()
    {
        
    }

    public override void FixedUpdate()
    {
        if (!atDestination)
        {
            base.FixedUpdate();
        }
    }

    IEnumerator Ripples()
    {
        yield return new WaitForSeconds(initialDelay);
        for(int i = 0; i < rippleNum; i++)
        {
            Instantiate(ripplePrefab).transform.position = transform.position + Vector3.up;
            yield return new WaitForSeconds(rippleDelay);
        }
    }
}

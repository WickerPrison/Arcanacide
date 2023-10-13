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
    [SerializeField] float rippleDelayTime;
    [SerializeField] Color rippleColor;
    WaitForSeconds rippleDelay;

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
        rippleDelay = new WaitForSeconds(rippleDelayTime);
        yield return new WaitForSeconds(initialDelay);
        for(int i = 0; i < rippleNum; i++)
        {
            IceRipple ripple = Instantiate(ripplePrefab).GetComponent<IceRipple>();
            ripple.transform.position = transform.position + Vector3.up;
            ripple.transform.localScale = Vector3.one * 0.5f;
            ripple.startRadius = 0.5f;
            ripple.numberOfBoxes = 20;
            ripple.rippleSpeed = 5;
            ripple.lifeTime = 2;
            ripple.boxColor = rippleColor;
            yield return rippleDelay;
        }
        yield return rippleDelay;
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossLimb : MonoBehaviour
{
    [SerializeField] int limbID;
    [SerializeField] bool isHuman;
    IceBossAnimationEvents events;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        events = GetComponentInParent<IceBoss>().animationEvents;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.enabled = isHuman;
    }

    private void OnEnable()
    {
        events.OnReplaceLimb += Events_OnReplaceLimb;
    }

    private void Events_OnReplaceLimb(int limb)
    {
        if( limb == limbID )
        {
            spriteRenderer.enabled = !isHuman;
        }
    }
}

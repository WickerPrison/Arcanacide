using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossLimb : MonoBehaviour
{
    [SerializeField] int limbID;
    [SerializeField] bool isHuman;
    IceBoss iceBoss;
    IceBossAnimationEvents events;
    SpriteRenderer spriteRenderer;
    MapData mapData;

    private void Awake()
    {
        iceBoss = GetComponentInParent<IceBoss>();
        mapData = iceBoss.mapData;
        events = iceBoss.animationEvents;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (mapData.iceBossKilled)
        {
            spriteRenderer.enabled = !isHuman;
        }
        else
        {
            spriteRenderer.enabled = isHuman;
        }
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

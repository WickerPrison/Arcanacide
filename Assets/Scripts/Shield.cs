using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color transparent;
    [SerializeField] Color solid;
    [SerializeField] Color barrier;
    
    PatchEffects patchEffects;
    PlayerAbilities playerAbilities;

    private void Start()
    {
        patchEffects = GetComponentInParent<PatchEffects>();
        playerAbilities = patchEffects.GetComponent<PlayerAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAbilities.shield)
        {
            spriteRenderer.enabled = true;
            if (playerAbilities.parry)
            {
                spriteRenderer.color = solid;
                //spriteRenderer.material.SetFloat("_PerlinSize", 0);
                spriteRenderer.material.SetFloat("_EdgeDecay", 0);
            }
            else
            {
                spriteRenderer.color = transparent;
                //spriteRenderer.material.SetFloat("_PerlinSize", 2);
                spriteRenderer.material.SetFloat("_EdgeDecay", 0.6f);
            }
        }
        else if (patchEffects.barrier)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = barrier;
            //spriteRenderer.material.SetFloat("_PerlinSize", 2);
            spriteRenderer.material.SetFloat("_EdgeDecay", 0.6f);
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}

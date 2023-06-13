using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color transparent;
    [SerializeField] Color solid;
    [SerializeField] Color barrier;

    // Update is called once per frame
    void Update()
    {
        if (playerScript.shield)
        {
            spriteRenderer.enabled = true;
            if (playerScript.parry)
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
        else if (playerScript.barrier)
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

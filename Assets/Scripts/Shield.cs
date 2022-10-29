using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color transparent;
    [SerializeField] Color solid;

    // Update is called once per frame
    void Update()
    {
        if (playerScript.shield)
        {
            spriteRenderer.enabled = true;
            if (playerScript.parry)
            {
                spriteRenderer.color = solid;
            }
            else
            {
                spriteRenderer.color = transparent;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}

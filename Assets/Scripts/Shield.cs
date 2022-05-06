using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        if (playerScript.shield)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}

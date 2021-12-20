using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        if (playerController.shield)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}

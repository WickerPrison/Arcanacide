using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportVFX : MonoBehaviour
{
    [SerializeField] Vector3 startScale;
    public float awayDuration = 0;
    float growRate;

    private void Update()
    {
        if(awayDuration > 0)
        {
            awayDuration -= Time.deltaTime;
            transform.localScale -= Vector3.one * growRate * Time.deltaTime;
            
            if(awayDuration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TeleportAway()
    {
        awayDuration = 1;
        transform.localScale = startScale;
        growRate = startScale.x / awayDuration;
    }
}

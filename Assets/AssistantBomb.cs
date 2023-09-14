using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantBomb : MonoBehaviour
{
    float timer;
    ArcProjectile arcProjectile;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        arcProjectile = GetComponent<ArcProjectile>();
        timer = arcProjectile.timeToHit / 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            spriteRenderer.sortingOrder = 0;
        }
    }
}

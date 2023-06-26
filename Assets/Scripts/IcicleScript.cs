using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IcicleScript : MonoBehaviour
{
    Projectile projectileScript;
    [SerializeField] ParticleSystem vfx;

    private void Awake()
    {
        projectileScript = GetComponent<Projectile>();
        projectileScript.direction = new Vector3(0, -1, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer != 8)
        {
            vfx.transform.position = transform.position;
            vfx.Play();
        }
    }
}

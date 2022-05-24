using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem vfx;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer != 8)
        {
            vfx.transform.position = transform.position;
            vfx.Play();
        }
    }
}

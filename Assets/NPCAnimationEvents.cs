using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationEvents : MonoBehaviour
{
    [SerializeField] AudioSource sfx;

    public void SoundEffect()
    {
        sfx.Play();
    }
}

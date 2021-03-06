using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] AudioClip footstep;
    [SerializeField] AudioClip swordSwoosh;
    [SerializeField] AudioClip pain;
    [SerializeField] AudioClip swordImpact;
    [SerializeField] List<AudioClip> otherSounds = new List<AudioClip>();

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }

    public void Footstep()
    {
        SFX.PlayOneShot(footstep, 0.25f);
    }

    public void SwordSwoosh()
    {
        SFX.PlayOneShot(swordSwoosh, 1);
    }

    public void SwordImpact()
    {
        SFX.PlayOneShot(swordImpact, 1f);
    }

    public void Pain()
    {
        SFX.PlayOneShot(pain, 0.05f);
    }

    public void OtherSounds(int indexNumber, float volume)
    {
        SFX.PlayOneShot(otherSounds[indexNumber], volume);
    }
}

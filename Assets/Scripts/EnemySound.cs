using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [System.NonSerialized] public AudioSource SFX;
    [SerializeField] EventReference footstep;
    [SerializeField] EventReference swordSwoosh;
    [SerializeField] EventReference swordImpact;
    [SerializeField] EventReference enemySpell;
    [SerializeField] EventReference blockAttack;
    [SerializeField] List<AudioClip> otherSounds = new List<AudioClip>();
    [SerializeField] EventReference[] otherSFX;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }

    public void Footstep()
    {
        RuntimeManager.PlayOneShot(footstep, 0.25f);
    }

    public void SwordSwoosh()
    {
        RuntimeManager.PlayOneShot(swordSwoosh, 1);
    }

    public void SwordImpact()
    {
        RuntimeManager.PlayOneShot(swordImpact, 1);
    }

    public void BlockAttack()
    {
        RuntimeManager.PlayOneShot(blockAttack, 1);
    }

    public void EnemySpell()
    {
        RuntimeManager.PlayOneShot(enemySpell, 1);
    }

    public void OtherSounds(int indexNumber, float volume)
    {
        RuntimeManager.PlayOneShot(otherSFX[indexNumber], volume, transform.position);
    }
}

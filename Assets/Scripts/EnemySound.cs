using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    [SerializeField] EventReference footstep;
    [SerializeField] EventReference swordSwoosh;
    [SerializeField] EventReference swordImpact;
    [SerializeField] EventReference enemySpell;
    [SerializeField] EventReference blockAttack;
    [SerializeField] EventReference electricChargeShock;
    [SerializeField] List<AudioClip> otherSounds = new List<AudioClip>();
    [SerializeField] EventReference[] otherSFX;
    EventInstance fmodInstance;

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

    public void ElectricShock()
    {
        RuntimeManager.PlayOneShot(electricChargeShock, 0.5f);
    }

    public void OtherSounds(int indexNumber, float volume)
    {
        RuntimeManager.PlayOneShot(otherSFX[indexNumber], volume, transform.position);
    }

    public void Play(EventReference fmodEvent, float volume) 
    {
        fmodInstance.release();
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.start();
        fmodInstance.setVolume(volume);
    }

    public void SetPaused(bool isPaused)
    {
        fmodInstance.setPaused(isPaused);
    }

    public void Stop()
    {
        fmodInstance.release();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDisable()
    {
        Stop();
    }

    private void OnDestroy()
    {
        Stop();
    }
}

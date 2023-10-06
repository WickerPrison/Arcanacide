using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource SFX;
    [SerializeField] AudioClip bossDefeated;
    [SerializeField] EventReference death;
    [SerializeField] EventReference buttonSFX;

    private void Start()
    {
        SFX = GetComponent<AudioSource>();
    }
    private void onBossKilled(object sender, System.EventArgs e)
    {
        SFX.PlayOneShot(bossDefeated, 1);
    }

    public void DeathSoundEffect()
    {
        RuntimeManager.PlayOneShot(death, 1f);
    }

    public void ButtonSound()
    {
        RuntimeManager.PlayOneShot(buttonSFX);
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += onBossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= onBossKilled;
    }
}

using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] EventReference bossDefeated;
    [SerializeField] EventReference death;
    [SerializeField] EventReference buttonSFX;

    private void onBossKilled(object sender, System.EventArgs e)
    {
        //RuntimeManager.PlayOneShot(bossDefeated);
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

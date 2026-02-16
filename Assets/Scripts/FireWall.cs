using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    [SerializeField] EventReference fmodEvent;
    [SerializeField] EventReference playerImpactSFX;
    [SerializeField] FireSuppressionSwitch fireSuppressionSwitch;
    [SerializeField] Color redColor;
    [SerializeField] SpriteRenderer[] grates;
    [SerializeField] ParticleSystem[] particles;
    [SerializeField] MapData mapData;
    Collider wallCollider;
    PlayerScript playerScript;
    EventInstance fmodInstance;
    public int damage;

    private void OnEnable()
    {
        fireSuppressionSwitch.onFixed += onFixed;
    }

    private void onFixed(object sender, System.EventArgs e)
    {
        foreach(SpriteRenderer grate in grates)
        {
            grate.color = Color.white;
        }
        foreach(ParticleSystem fire in particles)
        {
            fire.Stop();
        }
        wallCollider.enabled = false;
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }

    private void Start()
    {
        wallCollider = GetComponent<Collider>();
        if(mapData.fireSuppressionState != FireSuppressionState.FIXED)
        {
            foreach(SpriteRenderer grate in grates)
            {
                grate.color = redColor;
            }
            foreach (ParticleSystem fire in particles)
            {
                fire.Play();
            }
            wallCollider.enabled = true;
            fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
            //fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
            fmodInstance.start();
            fmodInstance.setTimelinePosition(Random.Range(0, 2000));
            fmodInstance.setVolume(0.1f);
        }
        else
        {
            foreach(SpriteRenderer grate in grates)
            {
                grate.color = Color.white;
            }
            foreach (ParticleSystem fire in particles)
            {
                fire.Stop();
                fire.Clear();
            }
            wallCollider.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(playerScript == null)
            {
                playerScript = collision.gameObject.GetComponent<PlayerScript>();
            }
            FmodUtils.PlayOneShot(playerImpactSFX, 1, transform.position);
            playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.StartStagger(0.5f);
        }
    }

    private void OnDisable()
    {
        fireSuppressionSwitch.onFixed -= onFixed;
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}

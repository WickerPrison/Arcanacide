using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPuddleScript : MonoBehaviour
{
    [SerializeField] List<Collider> colliders;
    [SerializeField] ParticleSystem particles;
    [SerializeField] bool startOn = false;
    [SerializeField] EventReference damageSoundEvent;
    [SerializeField] EventReference fmodEvent;
    [SerializeField] float volume;
    EventInstance fmodInstance;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Rigidbody playerRigidbody;
    float staggerDuration = 1;
    float staggerTimer;
    bool powerOn = false;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerSound = playerScript.gameObject.GetComponentInChildren<PlayerSound>();
        playerRigidbody = playerScript.gameObject.GetComponent<Rigidbody>();
        if (startOn)
        {
            PowerOn();
        }
        else
        {
            PowerOff();
        }
    }

    private void Update()
    {
        if(staggerTimer > 0)
        {
            staggerTimer -= Time.deltaTime;
            playerRigidbody.velocity = Vector3.zero;
        }
    }

    public void PowerOff()
    {
        powerOn = false;
        particles.Stop();
        fmodInstance.release();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    public void PowerOn()
    {
        if (powerOn)
        {
            return;
        }

        powerOn = true;
        particles.Play();
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        fmodInstance.start();
        fmodInstance.setVolume(volume);
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.LoseHealth(25, EnemyAttackType.NONPARRIABLE, null);
            playerScript.StartStagger(staggerDuration);
            playerSound.PlaySoundEffect(damageSoundEvent, 1);
            staggerTimer = staggerDuration;
        }
    }

    private void OnDisable()
    {
        fmodInstance.release();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}

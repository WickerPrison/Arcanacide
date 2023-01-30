using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPuddleScript : MonoBehaviour
{
    [SerializeField] List<Collider> colliders;
    [SerializeField] ParticleSystem particles;
    [SerializeField] bool startOn = false;
    [SerializeField] AudioClip damageSound;
    AudioSource sfx;
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
        sfx = GetComponent<AudioSource>();
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
        sfx.Stop();
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
        sfx.Play();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.LoseHealth(25);
            playerScript.StartStagger(staggerDuration);
            playerSound.PlaySoundEffect(damageSound, 1);
            staggerTimer = staggerDuration;
        }
    }
}

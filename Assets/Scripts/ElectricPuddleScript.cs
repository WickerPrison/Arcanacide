using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPuddleScript : MonoBehaviour
{
    [SerializeField] List<Collider> colliders;
    [SerializeField] ParticleSystem particles;
    [SerializeField] bool startOn = false;
    PlayerScript playerScript;
    Rigidbody playerRigidbody;
    float staggerDuration = 1;
    float staggerTimer;

    // Start is called before the first frame update
    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
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
        particles.Stop();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }

    public void PowerOn()
    {
        particles.Play();
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
            staggerTimer = staggerDuration;
        }
    }
}

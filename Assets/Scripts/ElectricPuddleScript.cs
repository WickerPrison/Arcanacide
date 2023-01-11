using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPuddleScript : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] int powerSwitchNumber;
    [SerializeField] List<Collider> colliders;
    [SerializeField] ParticleSystem particles;
    bool switchFlipped = false;
    PlayerScript playerScript;
    Rigidbody playerRigidbody;
    float staggerDuration = 1;
    float staggerTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerRigidbody = playerScript.gameObject.GetComponent<Rigidbody>();
        if (mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
            FlipSwitch();
        }
    }

    private void Update()
    {
        if(staggerTimer > 0)
        {
            staggerTimer -= Time.deltaTime;
            playerRigidbody.velocity = Vector3.zero;
        }

        if (!switchFlipped && mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
            FlipSwitch();
        }
    }

    void FlipSwitch()
    {
        switchFlipped = true;
        particles.Stop();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
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

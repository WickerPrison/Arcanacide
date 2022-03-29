using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] AudioClip impactSound;
    AudioSource sfx;
    Transform player;
    float duration = 5;
    int damage = 20;
    float poiseDamage = 70;
    float speed = 5;

    private void Start()
    {
        sfx = GetComponent<AudioSource>();
        sfx.time = Random.Range(0, 0.5f);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.layer == 3)
            {
                PlayerScript playerScript;
                playerScript = other.gameObject.GetComponent<PlayerScript>();
                playerScript.LoseHealth(damage);
                playerScript.LosePoise(poiseDamage);
                AudioSource.PlayClipAtPoint(impactSound, transform.position, 0.5f);
                Destroy(gameObject);
            }
            else if(other.gameObject.layer == 8)
            {
                PlayerController playerController;
                playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.PathOfTheSword();
            }
        }
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * Time.fixedDeltaTime * speed);
    }
}

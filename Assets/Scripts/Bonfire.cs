using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    Transform player;
    float duration = 5;
    int damage = 20;
    float poiseDamage = 70;
    float speed = 5;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript;
            playerScript = other.gameObject.GetComponent<PlayerScript>();
            playerScript.LoseHealth(damage);
            playerScript.LosePoise(poiseDamage);
            Destroy(gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACunitScript : MonoBehaviour
{
    PlayerScript playerScript;
    ParticleSystem particles;
    float playerDistance;
    [SerializeField] float range;
    [SerializeField] float damageRate;
    float damageCounter;
    [SerializeField] float staminaRate;
    float staminaCounter;
    [SerializeField] MapData mapData;

    // Start is called before the first frame update
    void Start()
    {
        if (!mapData.ACOn)
        {
            return;
        }

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        particles = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.startLifetime = range;
        particles.Simulate(range + 5);
        particles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapData.ACOn)
        {
            return;
        }

        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);
        if(playerDistance < range)
        {
            damageCounter += Time.deltaTime * damageRate;
            if(damageCounter > 1)
            {
                int amount = Mathf.FloorToInt(damageCounter);
                damageCounter -= amount;
                playerScript.LoseHealth(amount);
            }

            staminaCounter += Time.deltaTime * staminaRate;
            if(staminaCounter > 1)
            {
                int amount = Mathf.FloorToInt(staminaCounter);
                staminaCounter -= amount;
                playerScript.LoseStamina(amount);
            }
        }
    }
}

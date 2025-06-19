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
    [SerializeField] SpriteRenderer flashingLight;
    [SerializeField] SpriteRenderer stableLight;
    float lightMax = 4.5f;
    float lightMin = 1.5f;
    float slider = 4f;
    float sliderDir = -1;
    float sliderSpeed = 4;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        particles = GetComponentInChildren<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.startLifetime = range;
        Setup(mapData.ACOn);
    }

    void Setup(bool acOn)
    {
        if (!acOn)
        {
            flashingLight.material.SetFloat("_slider", 15);
            stableLight.material.SetFloat("_slider", 15);
            particles.Clear();
            particles.Stop();
        }
        else
        {
            stableLight.material.SetFloat("_slider", 1.5f);
            particles.Simulate(range + 5);
            particles.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapData.ACOn)
        {
            flashingLight.material.SetFloat("_slider", 15);
            stableLight.material.SetFloat("_slider", 15);
            return;
        }

        slider += sliderSpeed * Time.deltaTime * sliderDir;
        flashingLight.material.SetFloat("_slider", slider);
        if (slider > lightMax || slider < lightMin)
        {
            sliderDir *= -1;
        }


        playerDistance = Vector3.Distance(transform.position, playerScript.transform.position);
        if(playerDistance < range)
        {
            damageCounter += Time.deltaTime * damageRate;
            if(damageCounter > 1)
            {
                int amount = Mathf.FloorToInt(damageCounter);
                damageCounter -= amount;
                playerScript.LoseHealth(amount, EnemyAttackType.NONPARRIABLE, null);
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

    private void OnEnable()
    {
        GlobalEvents.instance.onSwitchAC += Global_onSwitchAC;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onSwitchAC -= Global_onSwitchAC;
    }

    private void Global_onSwitchAC(object sender, bool acOn)
    {
        Setup(acOn);
    }
}

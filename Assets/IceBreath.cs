using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreath : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] PlayerData playerData;
    ParticleSystem vfx;
    PlayerScript playerScript;
    float iceBreathRange = 4;
    LayerMask layerMask;
    RaycastHit[] hitTargets;
    float damage = 2;
    float damageCounter;
    float staminaCost = 5;
    bool iceBreathOn = false;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        vfx = GetComponent<ParticleSystem>();
        layerMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if (!iceBreathOn) return;

        playerScript.LoseStamina(staminaCost * Time.deltaTime);

        Vector3 lookDirection = attackPoint.position - transform.position;
        lookDirection = new Vector3 (lookDirection.x, 0, lookDirection.z).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        hitTargets = Physics.RaycastAll(transform.position, lookDirection, iceBreathRange, layerMask, QueryTriggerInteraction.Ignore);

        if(hitTargets.Length > 0 )
        {
            damageCounter += damage * Time.deltaTime * playerData.dedication;
            if (damageCounter > 1)
            {
                damageCounter = 0;
                foreach (RaycastHit hit in hitTargets )
                {
                    EnemyScript enemy = hit.collider.gameObject.GetComponent<EnemyScript>();
                    enemy.LoseHealth(1, 1);
                }                
            }
        }
    }

    public void StartIceBreath()
    {
        iceBreathOn = true;
        vfx.Play();
    }

    public void StopIceBreath()
    {
        iceBreathOn = false;
        vfx.Stop();
    }
}

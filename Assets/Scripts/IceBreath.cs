using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreath : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] PlayerData playerData;
    ParticleSystem vfx;
    PlayerScript playerScript;
    float iceBreathRange = 3;
    LayerMask layerMask;
    RaycastHit[] hitTargets;
    float damage = 2;
    float damageCounter;
    float staminaCost = 5;
    bool iceBreathOn = false;
    Vector3 offset = new Vector3(0, 1, 0);

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

        Vector3 lookDirection = attackPoint.position - playerScript.transform.position;
        lookDirection = new Vector3 (lookDirection.x, 0, lookDirection.z).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        hitTargets = Physics.RaycastAll(playerScript.transform.position + offset, lookDirection, iceBreathRange, layerMask, QueryTriggerInteraction.Ignore);

        //Debug.DrawRay(playerScript.transform.position, lookDirection * iceBreathRange, Color.red);

        if (hitTargets.Length > 0 )
        {
            damageCounter += damage * Time.deltaTime * playerData.arcane;
            if (damageCounter > 1)
            {
                damageCounter = 0;
                foreach (RaycastHit hit in hitTargets )
                {
                    EnemyScript enemy = hit.collider.gameObject.GetComponent<EnemyScript>();
                    enemy.LoseHealth(1, 0);
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

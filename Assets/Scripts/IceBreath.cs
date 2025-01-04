using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreath : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] PlayerData playerData;
    [SerializeField] AttackProfiles attackProfile;
    PlayerAbilities playerAbilities;
    [SerializeField] ParticleSystem vfx;
    PlayerScript playerScript;
    LayerMask layerMask;
    RaycastHit[] hitTargets;
    float damage;
    float timer;
    bool iceBreathOn = false;
    Vector3 offset = new Vector3(0, 1, 0);

    [SerializeField] EventReference fmodEvent;
    [SerializeField] float sfxVolume;
    EventInstance fmodInstance;

    private void Start()
    {
        if (!playerData.unlockedWeapons.Contains(3)) return;
        playerScript = GetComponentInParent<PlayerScript>();
        layerMask = LayerMask.GetMask("Enemy");
        fmodInstance = RuntimeManager.CreateInstance(fmodEvent);
        fmodInstance.setVolume(sfxVolume);
        playerAbilities = GetComponentInParent<PlayerAbilities>();
    }

    private void Update()
    {
        if (!iceBreathOn) return;

        playerScript.LoseStamina(attackProfile.staminaCost * Time.deltaTime);

        Vector3 lookDirection = attackPoint.position - playerScript.transform.position;
        lookDirection = new Vector3 (lookDirection.x, 0, lookDirection.z).normalized;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        hitTargets = Physics.RaycastAll(playerScript.transform.position + offset, lookDirection, attackProfile.attackRange, layerMask, QueryTriggerInteraction.Ignore);

        //Debug.DrawRay(playerScript.transform.position, lookDirection * iceBreathRange, Color.red);

        if (hitTargets.Length > 0 )
        {
            damage += playerAbilities.DetermineAttackDamage(attackProfile) * Time.deltaTime;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                foreach (RaycastHit hit in hitTargets )
                {
                    EnemyScript enemy = hit.collider.gameObject.GetComponent<EnemyScript>();
                    playerAbilities.DamageEnemy(enemy, Mathf.RoundToInt(damage), attackProfile);
                }
                timer = 0.3f;
                damage = 0;
            }
        }
    }

    public void StartIceBreath()
    {
        iceBreathOn = true;
        vfx.Play();
        fmodInstance.start();
    }

    public void StopIceBreath()
    {
        iceBreathOn = false;
        vfx.Stop();
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void OnDisable()
    {
        fmodInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        fmodInstance.release();
    }
}

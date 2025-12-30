using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnifeTrap : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [System.NonSerialized] public PlayerScript playerScript;
    PlayerAbilities playerAbilities;
    float damage = 0;
    float damagePerSecond;
    float charge;
    float timer;
    [System.NonSerialized] public Vector3 away = Vector3.one * -75;
    bool canMakeDamageSound = true;
    List<EnemyScript> enemiesInRange = new List<EnemyScript>();
    [System.NonSerialized] public StudioEventEmitter sfx;
    bool instantiatedCorrectly = false;
    AttackProfiles attackProfile;

    public static KnifeTrap Instantiate(GameObject prefab, AttackProfiles attackProfile, PlayerScript playerScript, PlayerAbilities playerAbilities)
    {
        KnifeTrap electricTrap = Instantiate(prefab).GetComponent<KnifeTrap>();
        electricTrap.attackProfile = attackProfile;
        electricTrap.playerScript = playerScript;
        electricTrap.playerAbilities = playerAbilities;
        electricTrap.instantiatedCorrectly = true;
        return electricTrap;
    }

    private void Awake()
    {
        sfx = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("KnifeTrap");
        damagePerSecond = playerAbilities.DetermineAttackDamage(attackProfile);
    }

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer < 0 )
            {
                EndTrap();
            }
        }

        //Debug.Log(enemiesInRange.Count);
        if(enemiesInRange.Count > 0)
        {
            damage += damagePerSecond * Time.deltaTime;
            if (damage > 1)
            {
                foreach(EnemyScript enemy in enemiesInRange)
                {
                    enemy.LoseHealthUnblockable(Mathf.FloorToInt(damage), 0);

                    if (playerData.equippedPatches.Contains(Patches.RENDING_BLOWS))
                    {
                        enemy.GainDOT((float)emblemLibrary.rendingBlows.value);
                    }
                }

                if (canMakeDamageSound)
                {
                    //RuntimeManager.PlayOneShot(electricDamage, 0.2f, transform.position);
                    RuntimeManager.PlayOneShot(attackProfile.soundOnHitEvent, attackProfile.soundOnHitVolume, transform.position);
                    StartCoroutine(SFXtimer());
                }
                damage = 0;
            }

            charge += attackProfile.electricChargeBuildup * Time.deltaTime;
            if(charge > 1)
            {
                int amount = Mathf.FloorToInt(charge);
                foreach (EnemyScript enemy in enemiesInRange)
                {
                    enemy.GainElectricCharge(amount);
                }
                charge = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<EnemyScript>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.GetComponent<EnemyScript>());
        }
    }

    IEnumerator SFXtimer()
    {
        canMakeDamageSound = false;
        yield return new WaitForSeconds(0.3f);
        canMakeDamageSound = true;
    }

    public void StartTimer()
    {
        timer = attackProfile.durationDOT;
        sfx.Play(); 
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onEnemyKilled += onEnmyKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onEnemyKilled -= onEnmyKilled;
    }

    private void onEnmyKilled(object sender, System.EventArgs e)
    {
        StartCoroutine(CheckForColliders());
    }

    IEnumerator CheckForColliders()
    {
        yield return new WaitForEndOfFrame();
        
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            if (enemiesInRange[i] == null) enemiesInRange.RemoveAt(i);
        }
    }

    public int GetEnemiesInRangeCount()
    {
        return enemiesInRange.Count;
    }

    public virtual void EndTrap()
    {
        transform.position = away;
        sfx.Stop();
        if (playerScript.testingEvents != null) playerScript.testingEvents.KnifeTrapDone();
    }
}

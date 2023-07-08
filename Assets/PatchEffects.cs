using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatchEffects : MonoBehaviour
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pathTrailPrefab;

    //player scripts
    PlayerEvents playerEvents;
    WeaponManager weaponManager;
    PlayerScript playerScript;
    PlayerAbilities playerAbilities;
    PlayerSound playerSound;

    //patch related variables
    [System.NonSerialized] public bool arcaneStepActive = false;
    [System.NonSerialized] public bool arcaneRemainsActive = false;
    float arcaneStepMaxTime = 0.03f;
    float arcaneStepTimer;

    float closeCallMaxTime = 5;
    [System.NonSerialized] public float closeCallTimer;

    [System.NonSerialized] public float mirrorCloakTimer;
    [System.NonSerialized] public float mirrorCloakMaxTime = 5;


    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
        playerScript = GetComponent<PlayerScript>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerSound = GetComponentInChildren<PlayerSound>();
    }

    private void Update()
    {
        if (closeCallTimer > 0)
        {
            closeCallTimer -= Time.deltaTime;
            if (closeCallTimer <= 0)
            {
                weaponManager.RemoveWeaponMagicSource();
            }
        }

        if (mirrorCloakTimer > 0)
        {
            mirrorCloakTimer -= Time.deltaTime;
            if (mirrorCloakTimer <= 0) playerEvents.StartMirrorCloak();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.arcane_step) && arcaneStepActive)
        {
            if (arcaneStepTimer < 0)
            {
                GameObject pathTrail;
                pathTrail = Instantiate(pathTrailPrefab);
                pathTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                arcaneStepTimer = arcaneStepMaxTime;
            }
            else
            {
                arcaneStepTimer -= Time.deltaTime;
            }
        }
    }

    public void PerfectDodge(GameObject projectile = null, EnemyScript attackingEnemy = null)
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call))
        {
            if (closeCallTimer <= 0)
            {
                weaponManager.AddWeaponMagicSource();
            }
            closeCallTimer = closeCallMaxTime;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.adrenaline_rush))
        {
            playerScript.stamina = playerData.MaxStamina();
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak) && mirrorCloakTimer <= 0 && attackingEnemy != null)
        {
            playerSound.Shield();

            playerAbilities.FireProjectile(attackingEnemy, new Vector3(transform.position.x, 1.1f, transform.position.z), playerScript.parryProfile);
        }
    }

    private void onPlayerStagger(object sender, System.EventArgs e)
    {
        EndArcaneStep();
    }

    public void StartArcaneStep()
    {
        arcaneStepActive = true;
        arcaneStepTimer = arcaneStepMaxTime;
    }

    public void EndArcaneStep()
    {
        arcaneStepActive = false;
        arcaneStepTimer = 0;
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += onPlayerStagger;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= onPlayerStagger;
    }
}

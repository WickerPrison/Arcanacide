using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;

    //player scripts
    PatchEffects patchEffects;
    PlayerAbilities playerAbilities;
    PlayerScript playerScript;
    PlayerEvents playerEvents;
    WeaponManager weaponManager;
    PlayerMovement playerMovement;
    PlayerAnimation playerAnimation;
    PlayerSound sfx;

    //other scripts
    GameManager gm;
    InputManager im;
    CameraFollow cameraScript;

    //variables
    [System.NonSerialized] public bool fullHealth;
    [System.NonSerialized] public float gemHealTimer = 0;
    float gemHealDuration = 2;
    float gemHealCounter = 0;
    float gemHealSpeed = 0;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        im = gm.gameObject.GetComponent<InputManager>();
        patchEffects = GetComponent<PatchEffects>();
        playerAbilities = GetComponent<PlayerAbilities>();
        playerScript = GetComponent<PlayerScript>();
        playerEvents = GetComponent<PlayerEvents>();
        weaponManager = GetComponent<WeaponManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        sfx = GetComponentInChildren<PlayerSound>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();

        if (playerData.health == playerData.MaxHealth())
        {
            fullHealth = true;
            if (playerData.equippedPatches.Contains(Patches.CONFIDENT_KILLER))
            {
                weaponManager.AddWeaponMagicSource();
            }
        }
    }

    private void Update()
    {
        if (gemHealTimer > 0)
        {
            gemHealTimer -= Time.deltaTime;
            if (gemHealTimer <= 0)
            {
                playerAnimation.EndBodyMagic();
            }
            gemHealCounter += gemHealSpeed * Time.deltaTime;
            if (gemHealCounter >= 1)
            {
                int amount = Mathf.FloorToInt(gemHealCounter);
                PartialHeal(amount);
                gemHealCounter -= amount;
            }
        }
    }

    public void LoseHealth(int damage, EnemyAttackType attackType, EnemyScript attackingEnemy)
    {
        if (patchEffects.barrier)
        {
            patchEffects.barrier = false;
            patchEffects.barrierTimer = patchEffects.maxBarrierTimer;
            return;
        }

        if (playerData.clawSpecialOn)
        {
            damage = Mathf.CeilToInt(damage * playerAbilities.clawSpecialTakeDamageMult);
        }

        if (damage > playerData.health && playerData.equippedPatches.Contains(Patches.ARCANE_PRESERVATION))
        {
            damage -= playerData.health;
            playerData.health = 1;
            playerScript.LoseMana(damage);
            damage = 0;
            if (playerData.mana <= 0)
            {
                playerData.health = 0;
            }
        }

        playerData.health -= damage;

        playerEvents.TakeDamage();
        GlobalEvents.instance.PlayerLoseHealth(playerData.health);

        if (attackType != EnemyAttackType.NONPARRIABLE) playerEvents.AttackImpact();

        if (attackingEnemy != null && playerData.equippedPatches.Contains(Patches.BURNING_CLOAK))
        {
            attackingEnemy.GainDOT(5);
        }

        if (fullHealth && playerData.equippedPatches.Contains(Patches.CONFIDENT_KILLER))
        {
            weaponManager.RemoveWeaponMagicSource();
        }
        fullHealth = false;

        float screenShakeMagnitude = (float)damage / (float)playerData.MaxHealth() * .1f;
        StartCoroutine(cameraScript.ScreenShake(.1f, screenShakeMagnitude));

        if (playerData.health <= 0)
        {
            im.DisableAll();
            playerMovement.preventInput = true;
            GlobalEvents.instance.OnPlayerDeath();
            YouDied youDied = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<YouDied>();
            youDied.playerScript = playerScript;
            youDied.ShowMessage();
        }
    }

    public void MaxHeal()
    {
        playerData.health = playerData.MaxHealth();
        if (!fullHealth && playerData.equippedPatches.Contains(Patches.CONFIDENT_KILLER))
        {
            weaponManager.AddWeaponMagicSource();
        }
        fullHealth = true;
        GlobalEvents.instance.PlayerGainHealth(playerData.health);
        sfx.PlaySoundEffect(PlayerSFX.HEAL, 0.6f);
    }

    public void PartialHeal(int healAmount)
    {
        playerData.health += healAmount;
        if (playerData.health >= playerData.MaxHealth())
        {
            playerData.health = playerData.MaxHealth();
            if (!fullHealth && playerData.equippedPatches.Contains(Patches.CONFIDENT_KILLER))
            {
                weaponManager.AddWeaponMagicSource();
            }
            fullHealth = true;
        }
        GlobalEvents.instance.PlayerGainHealth(playerData.health);
    }

    public void GemHeal()
    {
        if (playerData.healCharges < 0)
        {
            return;
        }

        playerData.healCharges -= 1;
        if (playerData.healCharges < 0 && playerData.tutorials.Contains("Broken Gem") && playerData.unlockedAbilities.Count > 0)
        {
            gm.GetComponent<TutorialManager>().Tutorial("Broken Gem");
        }

        if(gemHealTimer <= 0) playerAnimation.StartBodyMagic();

        gemHealTimer = gemHealDuration;
        gemHealSpeed = playerData.MaxHealth() / gemHealDuration;
        sfx.PlaySoundEffect(PlayerSFX.HEAL, 0.6f);

        if (playerData.equippedPatches.Contains(Patches.EXPLOSIVE_HEALING))
        {
            patchEffects.ExplosiveHealing();
        }

        GlobalEvents.instance.GemUsed();
    }

    private void Global_onPickupGemShard(object sender, int id)
    {
        
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onPickupGemShard += Global_onPickupGemShard;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onPickupGemShard -= Global_onPickupGemShard;
    }

}

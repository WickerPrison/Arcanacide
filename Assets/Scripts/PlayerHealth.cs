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
            if (playerData.equippedEmblems.Contains(emblemLibrary.confident_killer))
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

        if (damage > playerData.health && playerData.equippedEmblems.Contains(emblemLibrary.arcane_preservation))
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

        if (attackType != EnemyAttackType.NONPARRIABLE) playerEvents.AttackImpact();

        if (attackingEnemy != null && playerData.equippedEmblems.Contains(emblemLibrary.burning_cloak))
        {
            attackingEnemy.GainDOT(5);
        }

        if (fullHealth && playerData.equippedEmblems.Contains(emblemLibrary.confident_killer))
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
            YouDied youDied = GameObject.FindGameObjectWithTag("MainCanvas").GetComponentInChildren<YouDied>();
            youDied.playerScript = playerScript;
            youDied.ShowMessage();
            SoundManager sm = gm.gameObject.GetComponent<SoundManager>();
            sm.DeathSoundEffect();
            MusicManager musicManager = gm.gameObject.GetComponentInChildren<MusicManager>();
            musicManager.StartFadeOut(1);
        }
    }

    public void MaxHeal()
    {
        playerData.health = playerData.MaxHealth();
        if (!fullHealth && playerData.equippedEmblems.Contains(emblemLibrary.confident_killer))
        {
            weaponManager.AddWeaponMagicSource();
        }
        fullHealth = true;
        sfx.PlaySoundEffect(PlayerSFX.HEAL, 0.6f);
    }

    public void PartialHeal(int healAmount)
    {
        playerData.health += healAmount;
        if (playerData.health >= playerData.MaxHealth())
        {
            playerData.health = playerData.MaxHealth();
            if (!fullHealth && playerData.equippedEmblems.Contains(emblemLibrary.confident_killer))
            {
                weaponManager.AddWeaponMagicSource();
            }
            fullHealth = true;
        }
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

        gemHealTimer = gemHealDuration;
        gemHealSpeed = playerData.MaxHealth() / gemHealDuration;
        sfx.PlaySoundEffect(PlayerSFX.HEAL, 0.6f);
        playerAnimation.StartBodyMagic();

        if (playerData.equippedEmblems.Contains(emblemLibrary.explosive_healing))
        {
            patchEffects.ExplosiveHealing();
        }
    }

    private void onBossKilled(object sender, System.EventArgs e)
    {
        playerData.maxHealCharges++;
        playerData.healCharges++;
    }

    private void OnEnable()
    {
        GlobalEvents.instance.onBossKilled += onBossKilled;
    }

    private void OnDisable()
    {
        GlobalEvents.instance.onBossKilled -= onBossKilled;
    }

}

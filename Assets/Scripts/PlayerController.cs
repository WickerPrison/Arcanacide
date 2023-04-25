using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    //This script is responsible for accepting inputs from the player and performing actions 
    //that those inputs dictate

    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pauseMenuPrefab;
    [SerializeField] ParticleSystem shoveVFX;
    [SerializeField] GameObject totemObject;
    [SerializeField] Transform frontSwordTip;
    [SerializeField] Transform backSwordTip;
    [SerializeField] PlayerProjectile projectilePrefab;
    [SerializeField] List<AttackProfiles> specialAttackProfiles;
    [SerializeField] Bolts bolts;
    [SerializeField] Transform[] boltsOrigin;

    public ParticleSystem dodgeVFX;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float moveSpeed;
    public bool knockback = false;
    [System.NonSerialized] public bool canWalk = false;

    InputManager im;
    GameManager gm;
    SoundManager sm;
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    WeaponManager weaponManager;
    PlayerScript playerScript;
    PlayerSound playerSound;
    public Rigidbody rb;
    GameObject pauseMenu;
    Vector3 mouseDirection;
    [System.NonSerialized] public Vector3 moveDirection;
    [System.NonSerialized] public Vector3 dashDirection;
    float dashSpeed = 1000;
    [System.NonSerialized] public float dashTime = 0;
    [System.NonSerialized] public float maxDashTime = 0.2f;
    float dashStaminaCost = 30f;
    float lockOnDistance = 10;
    public bool preventInput = false;
    [System.NonSerialized] public bool lockPosition = false;

    float closeCallMaxTime = 5;
    [System.NonSerialized] public float closeCallTimer;
    float arcaneStepMaxTime = 0.03f;
    float arcaneStepTimer;
    [System.NonSerialized] public bool arcaneStepActive = false;
    [System.NonSerialized] public bool arcaneRemainsActive = false;
    [SerializeField] GameObject pathTrailPrefab;

    [System.NonSerialized] public float axeHeavyTimer = 0;
    [System.NonSerialized] public float axeHeavyMaxTime = 15;

    float shoveManaCost = 20;
    float shoveRadius = 3;
    float shovePoiseDamage = 100;

    WaitForSeconds parryWindow = new WaitForSeconds(0.1f);
    float parryCost = 20;

    Vector2 rightStickValue;
    public Vector2 lookDir;
    List<Transform> nearbyEnemies = new List<Transform>();

    bool knifeSpecialAttackOn = false;
    float boltdamage = 0;
    Vector3 away = Vector3.one * 100;

    bool heavyAttackActive = false;
    float clawSpecialMaxTime = 15f;
    float clawSpecialDamageMult = 2;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
        //bolts.ToggleBolts(false);
        bolts.SetPositions(away, away);
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sm = gm.gameObject.GetComponent<SoundManager>();
        SetUpControls();
        //Set references to other player scripts
        playerAnimation = GetComponent<PlayerAnimation>();
        weaponManager = GetComponent<WeaponManager>();
        playerScript = GetComponent<PlayerScript>();
        playerSound = GetComponentInChildren<PlayerSound>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //turn those inputs into a vector that cannot have a magnitude greater than 1
        moveDirection = new Vector3(playerData.moveDir.x, 0, playerData.moveDir.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        AttackPointPosition();

        if(axeHeavyTimer > 0)
        {
            axeHeavyTimer -= Time.deltaTime;
            if(axeHeavyTimer <= 0)
            {
                weaponManager.RemoveSpecificWeaponSource(1);
            }
        }

        if (closeCallTimer > 0)
        {
            closeCallTimer -= Time.deltaTime;
            if (closeCallTimer <= 0)
            {
                weaponManager.RemoveWeaponMagicSource();
            }
        }

        if (lockPosition)
        {
            rb.velocity = Vector3.zero;
        }

        if (knifeSpecialAttackOn)
        {
            UpdateKnifeSpecialAttack();
        }

        if(playerData.clawSpecialTimer > 0)
        {
            playerData.clawSpecialTimer -= Time.deltaTime;
            if(playerData.clawSpecialTimer <= 0)
            {
                playerData.clawSpecialOn = false;
                playerEvents.EndClawSpecialAttack();
            }
        }
    }

    //FixedUpdate is similar to Update but should always be used when dealing with physics
    private void FixedUpdate()
    {
        if (moveDirection.magnitude > 0 && (CanInput() || canWalk))
        {
            playerAnimation.walk = true;
        }
        else
        {
            playerAnimation.walk = false;
        }

        //move the player if they are not dashing or attacking
        if (CanInput() || canWalk)
        {
            rb.velocity = new Vector3(moveDirection.x * Time.fixedDeltaTime * moveSpeed, rb.velocity.y, moveDirection.z * Time.fixedDeltaTime * moveSpeed);
        }
        //dash if the player has pressed the right mouse button
        else if (dashTime > 0)
        {
            Dash();
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

    void Shield()
    {
        if (!playerData.unlockedAbilities.Contains("Block") || !CanInput())
        {
            return;
        }

        if (playerData.mana > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.PlayAnimation("Block");
            playerAnimation.continueBlocking = true;
        }
    }

    void Shove()
    {
        if (!playerData.unlockedAbilities.Contains("Shove") || !CanInput())
        {
            return;
        }

        if (playerData.mana > shoveManaCost)
        {
            playerScript.LoseMana(shoveManaCost);
            rb.velocity = Vector3.zero;
            playerAnimation.PlayAnimation("Shove");
        }
    }

    public void ShoveEffect()
    {
        shoveVFX.Play();
        foreach (EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= shoveRadius)
            {
                enemy.LosePoise(shovePoiseDamage);
                EnemyController enemyController = enemy.gameObject.GetComponent<EnemyController>();
                enemyController.StartStagger(0.5f);
            }
        }
    }

    void Dodge()
    {
        if (CanInput() && playerScript.stamina > 0 && moveDirection.magnitude > 0)
        {
            //The player dashes in whatever direction they were already moving
            float staminaCost = dashStaminaCost;
            dodgeVFX.Play();
            dashDirection = moveDirection.normalized;
            dashTime = maxDashTime;
            if (playerData.equippedEmblems.Contains(emblemLibrary.quickstep_))
            {
                staminaCost /= 2;
            }

            if (playerData.equippedEmblems.Contains(emblemLibrary.shell_company))
            {
                staminaCost *= 2;
            }

            playerScript.LoseStamina(staminaCost);
            playerAnimation.PlayAnimation("Dash");
            playerSound.Dodge();
        }
    }

    void Attack()
    {
        if (playerScript.shield && !playerScript.parry)
        {
            StartCoroutine(Parry());
            return;
        }

        if (playerAnimation.attacking)
        {
            playerAnimation.ChainAttacks();
        }
        else if (CanInput() && playerScript.stamina > 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = true;
            playerAnimation.PlayAnimation("Attack");
        }
    }

    void HeavyAttack()
    {
        if (playerAnimation.attacking)
        {
            playerAnimation.Combo();
        }
        else if (CanInput() && playerScript.stamina > 0)
        {
            heavyAttackActive = true;
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = playerData.currentWeapon != 3;
            playerAnimation.PlayAnimation("HeavyAttack");
        }
    }

    void EndHeavyAttack()
    {
        if(playerData.currentWeapon == 3 && heavyAttackActive)
        {
            playerAnimation.PlayAnimation("EndHeavyAttack");
        }

        heavyAttackActive = false;
    }

    public void SpecialAttack()
    {
        if (!playerData.unlockedAbilities.Contains("Special Attack")) return;

        if(CanInput() && playerScript.stamina > 0 && playerData.mana > specialAttackProfiles[playerData.currentWeapon].manaCost)
        {
            if(playerData.currentWeapon == 1)
            {
                AxeSpecialAttack();
            }
            else
            {
                rb.velocity = Vector3.zero;
                playerAnimation.attacking = true;
                playerAnimation.PlayAnimation("SpecialAttack");
            }
        }
    }

    void EndSpecialAttack()
    {
        if(playerData.currentWeapon == 2)
        {
            knifeSpecialAttackOn = false;
            bolts.SetPositions(away, away);
            bolts.SoundOff();
            playerAnimation.PlayAnimation("EndSpecialAttack");
        }
    }

    public void SwordSpecialAttack()
    {
        playerScript.LoseStamina(specialAttackProfiles[0].staminaCost);
        playerScript.LoseMana(specialAttackProfiles[0].manaCost);
        Transform origin;
        if (playerAnimation.facingFront)
        {
            origin = frontSwordTip;
        }
        else
        {
            origin = backSwordTip;
        }

        foreach(EnemyScript enemy in gm.enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) <= specialAttackProfiles[0].attackRange)
            {
                FireProjectile(enemy, origin.position, specialAttackProfiles[0]);
            }
        }
    }

    public void AxeSpecialAttack()
    {
        playerScript.LoseMana(specialAttackProfiles[1].manaCost);
        GameObject totem = Instantiate(totemObject);
        totem.transform.position = new Vector3(transform.position.x, 0, transform.position.z);    
    }

    public void KnifeSpecialAttack()
    {
        knifeSpecialAttackOn = true;
    }

    void UpdateKnifeSpecialAttack()
    {
        playerData.mana -= Time.deltaTime * specialAttackProfiles[2].manaCost;
        if (playerData.mana <= 0)
        {
            EndSpecialAttack();
            return;
        }

        EnemyScript closestEnemy = null;
        float distance = 10;
        foreach (EnemyScript enemy in gm.enemies)
        {
            float enemyDistance = Vector3.Distance(enemy.transform.position, transform.position);
            if (enemyDistance < distance)
            {
                distance = enemyDistance;
                closestEnemy = enemy;
            }
        }

        int boltsFrontOrBack = 0;
        if (playerAnimation.facingDirection > 1)
        {
            boltsFrontOrBack = 1;
        }

        if (closestEnemy != null)
        {
            bolts.SetPositions(boltsOrigin[boltsFrontOrBack].position, closestEnemy.transform.position + new Vector3(0, 1.1f, 0));
            bolts.SoundOn();
            boltdamage += playerData.dedication * specialAttackProfiles[2].magicDamageMultiplier * Time.deltaTime;
            if (boltdamage > 1)
            {
                closestEnemy.LoseHealth(Mathf.FloorToInt(boltdamage), 0);
                boltdamage = 0;
            }
        }
        else
        {
            bolts.SetPositions(away, away);
            bolts.SoundOff();
        }
    }

    public void FireProjectile(EnemyScript enemy, Vector3 startingPosition, AttackProfiles attackProfile)
    {
        PlayerProjectile projectile = Instantiate(projectilePrefab).GetComponent<PlayerProjectile>();
        projectile.attackProfile = attackProfile;
        projectile.transform.position = startingPosition;
        projectile.transform.LookAt(enemy.transform.position + new Vector3(0, 1.1f, 0));
        projectile.target = enemy.transform;
        projectile.playerController = this;
    }

    IEnumerator Parry()
    {
        playerScript.LoseMana(parryCost);
        playerScript.parry = true;
        yield return parryWindow;
        playerScript.parry = false;
    }

    public int DamageModifiers(int attackPower)
    {
        if(playerData.currentWeapon == 1 && axeHeavyTimer > 0)
        {
            attackPower += playerData.ArcaneDamage();
        }

        if(playerData.clawSpecialOn)
        {
            attackPower = Mathf.RoundToInt(attackPower * clawSpecialDamageMult);
        }

        return attackPower;
    }

    //The attack point is used to determine if an attack hits. It always stays between the player and the mouse
    void AttackPointPosition()
    {
        if (!CanInput() && !canWalk)
        {
            return;
        }

        if (Gamepad.current == null)
        {
            mouseDirection = playerAnimation.mousePosition - playerAnimation.playerScreenPosition;
            mouseDirection = new Vector3(mouseDirection.x, 0, mouseDirection.y);
            attackPoint.transform.position = transform.position + mouseDirection.normalized;
            attackPoint.transform.rotation = Quaternion.LookRotation(mouseDirection.normalized);
        }
        else
        {
            if (playerData.moveDir.magnitude != 0)
            {
                lookDir = playerData.moveDir;
            }
            LockOn();
            if (rightStickValue.magnitude > 0)
            {
                lookDir = rightStickValue.normalized;
            }
            Vector3 lookDirection = new Vector3(lookDir.x, 0, lookDir.y);
            attackPoint.transform.position = transform.position + lookDirection.normalized;
            if(lookDirection.normalized != Vector3.zero)
            {
                attackPoint.transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
            }
        }
    }

    void LockOn()
    {
        if (gm.enemies.Count < 1)
        {
            return;
        }
        Transform lockOnTarget = transform;
        float currentDistance = lockOnDistance;
        for (int enemy = 0; enemy < gm.enemies.Count; enemy++)
        {
            if (Vector3.Distance(transform.position, gm.enemies[enemy].transform.position) < currentDistance)
            {
                lockOnTarget = gm.enemies[enemy].transform;
                currentDistance = Vector3.Distance(transform.position, gm.enemies[enemy].transform.position);
            }
        }
        if (lockOnTarget != transform)
        {
            Vector3 lockOnDirection = lockOnTarget.position - transform.position;
            lookDir = new Vector2(lockOnDirection.x, lockOnDirection.z);
        }
    }

    public void PauseMenu()
    {
        sm.ButtonSound();
        if (!pauseMenu)
        {
            preventInput = true;
            im.Menu();
            Time.timeScale = 0;
            pauseMenu = Instantiate(pauseMenuPrefab);
        }
        else
        {
            preventInput = false;
            im.Gameplay();
            Time.timeScale = 1;
            Destroy(pauseMenu);
        }
    }

    //Returns a bool that is true if the player is currently allowed to give new inputs
    public bool CanInput()
    {
        if (preventInput)
        {
            return false;
        }
        if (dashTime > 0)
        {
            return false;
        }
        if (playerAnimation.attacking)
        {
            return false;
        }
        if (playerScript.isStaggered)
        {
            return false;
        }
        return true;
    }

    //this funciton will be repeatedly called while dash time is greater than 0
    //dashSpeed controls how fast the player moves during a dash
    //maxDashTime controls how long the dash lasts
    void Dash()
    {
        rb.velocity = (dashDirection * Time.fixedDeltaTime * dashSpeed);
        dashTime -= Time.fixedDeltaTime;
    }


    public void PerfectDodge()
    {
        //playerSound.PerfectDodge();

        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call))
        {
            if(closeCallTimer <= 0)
            {
                weaponManager.AddWeaponMagicSource();
            }
            closeCallTimer = closeCallMaxTime;
        }

        if (playerData.equippedEmblems.Contains(emblemLibrary.adrenaline_rush))
        {
            playerScript.stamina = playerData.MaxStamina();
        }
    }

    public IEnumerator KnockBack(float knockbackTime)
    {
        knockback = false;
        yield return new WaitForSeconds(knockbackTime);
        knockback = true;
        rb.velocity = Vector3.zero;
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

    private void PlayerEvents_onPlayerStagger(object sender, System.EventArgs e)
    {
        EndArcaneStep();
        if (!knockback)
        {
            rb.velocity = Vector3.zero;
        }

        if (playerData.currentWeapon == 2 && knifeSpecialAttackOn)
        {
            knifeSpecialAttackOn = false;
            bolts.SetPositions(away, away);
        }
    }

    private void PlayerEvents_onClawSpecial(object sender, System.EventArgs e)
    {
        playerScript.LoseMana(specialAttackProfiles[3].manaCost);
        playerData.clawSpecialTimer = clawSpecialMaxTime;
        playerData.clawSpecialOn = true;
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += PlayerEvents_onPlayerStagger;
        playerEvents.onClawSpecial += PlayerEvents_onClawSpecial;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= PlayerEvents_onPlayerStagger;
        playerEvents.onClawSpecial -= PlayerEvents_onClawSpecial;
    }

    void SetUpControls()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        im.controls.Gameplay.Attack.performed += ctx => Attack();
        im.controls.Gameplay.HeavyAttack.performed += ctx => HeavyAttack();
        im.controls.Gameplay.HeavyAttack.canceled += ctx => EndHeavyAttack();
        im.controls.Gameplay.SpecialAttack.performed += ctx => SpecialAttack();
        im.controls.Gameplay.SpecialAttack.canceled += ctx => EndSpecialAttack();
        im.controls.Gameplay.Dodge.performed += ctx => Dodge();
        im.controls.Gameplay.PauseMenu.performed += ctx => PauseMenu();
        im.controls.Gameplay.Move.performed += ctx => playerData.moveDir = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Move.canceled += ctx => playerData.moveDir = Vector2.zero;
        im.controls.Gameplay.Shield.performed += ctx => Shield();
        im.controls.Gameplay.Shield.canceled += ctx => playerAnimation.continueBlocking = false;
        im.controls.Gameplay.Heal.performed += ctx => playerAnimation.HealAnimation();
        im.controls.Gameplay.Look.performed += ctx => rightStickValue = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Look.canceled += ctx => rightStickValue = Vector2.zero;
    }
}

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //This script is responsible for accepting inputs from the player and performing actions 
    //that those inputs dictate

    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pauseMenuPrefab;
    [SerializeField] ParticleSystem shoveVFX;
    [SerializeField] ParticleSystem dodgeVFX;
    [SerializeField] GameObject totemObject;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float moveSpeed;
    public bool knockback = false;

    InputManager im;
    GameManager gm;
    SoundManager sm;
    PlayerAnimation playerAnimation;
    PlayerScript playerScript;
    PlayerSound playerSound;
    public Rigidbody rb;
    GameObject pauseMenu;
    Vector3 mouseDirection;
    Vector3 moveDirection;
    Vector3 dashDirection;
    float dashSpeed = 1000;
    public float dashTime = 0;
    float maxDashTime = 0.2f;
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

    float totemManaCost = 25;
    float totemMaxCooldown = 2;
    float totemCooldown;
    float shoveManaCost = 20;
    float shoveRadius = 3;
    float shovePoiseDamage = 100;

    WaitForSeconds parryWindow = new WaitForSeconds(0.1f);
    float parryCost = 20;

    Vector2 rightStickValue;
    public Vector2 lookDir;
    List<Transform> nearbyEnemies = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sm = gm.gameObject.GetComponent<SoundManager>();
        SetUpControls();
        //Set references to other player scripts
        playerAnimation = GetComponent<PlayerAnimation>();
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

        if (closeCallTimer > 0)
        {
            closeCallTimer -= Time.deltaTime;
            if (closeCallTimer <= 0)
            {
                playerAnimation.EndSwordMagic();
            }
        }

        if (totemCooldown > 0)
        {
            totemCooldown -= Time.deltaTime;
        }

        if (lockPosition)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Shield()
    {
        if (!playerData.unlockedAbilities.Contains("Block") || !CanInput())
        {
            return;
        }

        if (playerData.mana > 0 && totemCooldown <= 0)
        {
            rb.velocity = Vector3.zero;
            playerAnimation.Shield();
            playerAnimation.continueBlocking = true;
        }
    }

    void Totem()
    {
        if (!playerData.unlockedAbilities.Contains("Totem") || !CanInput())
        {
            return;
        }

        if (playerData.mana > 0 && totemCooldown <= 0)
        {
            playerScript.LoseMana(totemManaCost);
            totemCooldown = totemMaxCooldown;
            GameObject totem = Instantiate(totemObject);
            totem.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
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
            playerAnimation.Shove();
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
            playerAnimation.DashAnimation();
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
            playerAnimation.Attack();
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
            rb.velocity = Vector3.zero;
            playerAnimation.attacking = true;
            playerAnimation.HeavyAttack();
        }
    }

    IEnumerator Parry()
    {
        playerScript.LoseMana(parryCost);
        playerScript.parry = true;
        yield return parryWindow;
        playerScript.parry = false;
    }

    public int AttackPower()
    {
        int attackPower;
        attackPower = playerData.AttackPower();

        if (playerData.equippedEmblems.Contains(emblemLibrary.quick_strikes))
        {
            attackPower = Mathf.RoundToInt(attackPower * 0.8f);
        }

        return attackPower;
    }

    //The attack point is used to determine if an attack hits. It always stays between the player and the mouse
    void AttackPointPosition()
    {
        if (!CanInput())
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


    //FixedUpdate is similar to Update but should always be used when dealing with physics
    private void FixedUpdate()
    {
        if (moveDirection.magnitude > 0 && CanInput())
        {
            playerAnimation.walk = true;
        }
        else
        {
            playerAnimation.walk = false;
        }

        //move the player if they are not dashing or attacking
        if (CanInput())
        {
            rb.velocity = new Vector3(moveDirection.x * Time.fixedDeltaTime * moveSpeed, rb.velocity.y, moveDirection.z * Time.fixedDeltaTime * moveSpeed);
        }
        //dash if the player has pressed the right mouse button
        else if (dashTime > 0)
        {
            Dash();
        }

        if (playerAnimation.attacking)
        {
            //rb.velocity = Vector3.zero;
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

    public void PerfectDodge()
    {
        //playerSound.PerfectDodge();

        if (playerData.equippedEmblems.Contains(emblemLibrary.close_call))
        {
            if(closeCallTimer <= 0)
            {
                playerAnimation.StartSwordMagic();
            }
            closeCallTimer = closeCallMaxTime;
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

    void SetUpControls()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        im.controls.Gameplay.Attack.performed += ctx => Attack();
        im.controls.Gameplay.HeavyAttack.performed += ctx => HeavyAttack();
        im.controls.Gameplay.Dodge.performed += ctx => Dodge();
        im.controls.Gameplay.PauseMenu.performed += ctx => PauseMenu();
        im.controls.Gameplay.Move.performed += ctx => playerData.moveDir = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Move.canceled += ctx => playerData.moveDir = Vector2.zero;
        im.controls.Gameplay.Shield.performed += ctx => Shield();
        im.controls.Gameplay.Shield.canceled += ctx => playerAnimation.continueBlocking = false;
        im.controls.Gameplay.Heal.performed += ctx => playerScript.Heal();
        im.controls.Gameplay.Look.performed += ctx => rightStickValue = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Look.canceled += ctx => rightStickValue = Vector2.zero;
        im.controls.Gameplay.Totem.performed += ctx => Totem();
    }
}

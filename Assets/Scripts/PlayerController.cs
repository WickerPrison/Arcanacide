using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //This script is responsible for accepting inputs from the player and performing actions 
    //that those inputs dictate

    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pauseMenuPrefab;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float moveSpeed;
    public float stagger;
    public float maxStaggered;
    public bool knockback = false;

    InputManager im;
    GameManager gm;
    SoundManager sm;
    PlayerAnimation playerAnimation;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Rigidbody rb;
    GameObject pauseMenu;
    Vector3 mouseDirection;
    Vector3 moveDirection;
    Vector3 dashDirection;
    float dashSpeed = 1000;
    public float dashTime = 0;
    float maxDashTime = 0.2f;
    float dashStaminaCost = 30f;
    string healString = "Heal";
    string blockString = "Block";
    float blockCD = 3;
    float healCD = 30;
    float attackPointRadius = 1.5f;
    float hitboxRadius = 1.5f;
    float lockOnDistance = 10;
    public bool preventInput = false;
    public bool shield;

    public bool pathActive = false;
    float swordMaxTime = 5;
    float swordTimer;
    float pathOfPathMaxTime = 0.04f;
    float pathOfPathTimer;
    [SerializeField] GameObject pathTrailPrefab;

    Vector2 moveDir;
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
        moveDirection = new Vector3(moveDir.x, 0, moveDir.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        if (playerAnimation.continueBlocking)
        {
            playerData.duckCD = blockCD;
        }

        if (playerData.healCharges > 0 && playerData.duckCD > 0)
        {
            playerData.duckCD -= Time.deltaTime;
            if (playerData.equippedEmblems.Contains(emblemLibrary.magical_acceleration))
            {
                playerData.duckCD -= Time.deltaTime;
            }
        }

        float scrollWheel = Mouse.current.scroll.ReadValue().y;
        if(scrollWheel > 0)
        {
            NextAbility();
        }
        else if(scrollWheel < 0)
        {
            PreviousAbility();
        }

        AttackPointPosition();

        if(stagger > 0)
        {
            if (!knockback)
            {
                rb.velocity = Vector3.zero;
            }
            stagger -= Time.deltaTime;
            if(stagger <= 0)
            {
                knockback = false;
                playerScript.ResetPoise();
            }
        }

        playerAnimation.StaggerUpdate(stagger);

        if(swordTimer > 0)
        {
            swordTimer -= Time.deltaTime;
            if(swordTimer <= 0)
            {
                pathActive = false;
                playerAnimation.EndSwordMagic();
            }
        }

        if(playerData.path == "Path" && pathActive)
        {
            if(pathOfPathTimer < 0)
            {
                PathOfThePath();
            }
            else
            {
                pathOfPathTimer -= Time.deltaTime;
            }
        }
    }

    void EquipAbility(string ability)
    {
        if (playerData.unlockedAbilities.Contains(ability))
        {
            playerData.equippedAbility = ability;
        }
    }

    void UseAbility()
    {
        if (playerData.duckCD <= 0)
        {
            rb.velocity = Vector3.zero;
            if (playerData.equippedAbility != healString)
            {
                playerAnimation.UseDuck(playerData.equippedAbility);
            }
            else
            {
                playerScript.DuckHeal();
            }
            switch (playerData.equippedAbility)
            {
                case "Heal":
                    playerData.healCharges -= 1;
                    playerData.duckCD += 30;
                    break;
                case "Block":
                    playerData.duckCD += blockCD;
                    break;
            }
            playerAnimation.continueBlocking = true;
        }
    }

    void NextAbility()
    {
        int index;
        index = playerData.unlockedAbilities.IndexOf(playerData.equippedAbility);
        index += 1;
        if (index > playerData.unlockedAbilities.Count - 1)
        {
            index = 0;
        }
        playerData.equippedAbility = playerData.unlockedAbilities[index]; 
    }

    void PreviousAbility()
    {
        int index;
        index = playerData.unlockedAbilities.IndexOf(playerData.equippedAbility);
        index -= 1;
        if (index < 0)
        {
            index = playerData.unlockedAbilities.Count - 1;
        }
        playerData.equippedAbility = playerData.unlockedAbilities[index];
    }

    void Dodge()
    {
        if (CanInput() && playerScript.stamina > 0 && moveDirection.magnitude > 0)
        {
            //They player dashes in whatever direction they were already moving
            dashDirection = moveDirection.normalized;
            dashTime = maxDashTime;
            if (playerData.equippedEmblems.Contains(emblemLibrary.quickstep_))
            {
                playerScript.LoseStamina(dashStaminaCost / 2);
            }
            else
            {
                playerScript.LoseStamina(dashStaminaCost);
            }
            playerAnimation.DashAnimation();
            playerSound.Dodge();
        }
    }

    void Attack()
    {
        if (playerAnimation.attacking)
        {
            playerAnimation.ChainAttacks();
        }
        else if (CanInput() && playerScript.stamina > 0)
        {
            playerAnimation.attack = true;
            playerAnimation.attacking = true;
        }
    }

    public int AttackPower()
    {
        int attackPower;
        attackPower = playerData.AttackPower();
        if (!pathActive)
        {
            return attackPower;
        }
        if(playerData.path == "Sword" || playerData.path == "Dying")
        {
            attackPower += playerData.PathDamage();
        }
        return attackPower;
    }

    public void Parry(EnemyScript enemyScript)
    {
        playerSound.SwordClang();
        playerAnimation.ParryAnimation();
        enemyScript.LosePoise(playerData.AttackPower());
    }

    public Collider[] HitBox()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, hitboxRadius, enemyLayers);
        return hitEnemies;
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
            mouseDirection = new Vector3(mouseDirection.x, 0.5f, mouseDirection.y);
            attackPoint.transform.position = transform.position + mouseDirection.normalized * attackPointRadius;
        }
        else
        {
            if (moveDir.magnitude != 0)
            {
                lookDir = moveDir;
            }
            LockOn();
            if(rightStickValue.magnitude > 0)
            {
                lookDir = rightStickValue.normalized;
            }
            Vector3 lookDirection = new Vector3(lookDir.x, 0.5f, lookDir.y);
            attackPoint.transform.position = transform.position + lookDirection.normalized * attackPointRadius;
        }
    }

    void LockOn()
    {
        if(gm.enemies.Count < 1)
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
        if(lockOnTarget != transform)
        {
            Vector3 lockOnDirection = lockOnTarget.position - transform.position;
            lookDir = new Vector2(lockOnDirection.x, lockOnDirection.z);
        }
    }

    void PauseMenu()
    {
        sm.ButtonSound();
        if (!pauseMenu)
        {
            preventInput = true;
            pauseMenu = Instantiate(pauseMenuPrefab);
        }
        else
        {
            preventInput = false;
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
        if(dashTime > 0)
        {
            return false;
        }
        if (playerAnimation.attacking)
        {
            return false;
        }
        if (stagger > 0)
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
        if(moveDirection.magnitude > 0 && CanInput())
        {
            playerAnimation.walk = true;
        }
        else
        {
            playerAnimation.walk = false;
        }

        //move the player if they are not dashing or attacking
        if(CanInput())
        {
            rb.velocity = new Vector3(moveDirection.x * Time.fixedDeltaTime * moveSpeed, rb.velocity.y , moveDirection.z * Time.fixedDeltaTime * moveSpeed);
        }
        //dash if the player has pressed the right mouse button
        else if(dashTime > 0)
        {
            Dash();
        }

        if (playerAnimation.attacking)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void PathOfTheSword()
    {
        if(playerData.path != "Sword")
        {
            return;
        }
        pathActive = true;
        swordTimer = swordMaxTime;
        playerAnimation.StartSwordMagic();
    }

    public void PathOfTheDying()
    {
        playerAnimation.StartSwordMagic();
        pathActive = true;
    }

    public void EndPathOfTheDying()
    {
        playerAnimation.EndSwordMagic();
        pathActive = false;
    }

    public void StartPathOfThePath()
    {
        if(playerData.path == "Path")
        {
            pathActive = true;
            pathOfPathTimer = pathOfPathMaxTime;
        }
    }

    void PathOfThePath()
    {
        GameObject pathTrail;
        pathTrail = Instantiate(pathTrailPrefab);
        pathTrail.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        pathOfPathTimer = pathOfPathMaxTime;
    }

    public void EndPathOfThePath()
    {
        if (playerData.path == "Path")
        {
            pathActive = false;
            pathOfPathTimer = 0;
        }
    }

    void SetUpControls()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();

        im.controls.Gameplay.Attack.performed += ctx => Attack();
        im.controls.Gameplay.Dodge.performed += ctx => Dodge();
        im.controls.Gameplay.PauseMenu.performed += ctx => PauseMenu();
        im.controls.Gameplay.Move.performed += ctx => moveDir = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Move.canceled += ctx => moveDir = Vector2.zero;
        im.controls.Gameplay.UseAbility.performed += ctx => UseAbility();
        im.controls.Gameplay.UseAbility.canceled += ctx => playerAnimation.continueBlocking = false;
        im.controls.Gameplay.EquipHeal.performed += ctx => EquipAbility(healString);
        im.controls.Gameplay.EquipBlock.performed += ctx => EquipAbility(blockString);
        im.controls.Gameplay.Look.performed += ctx => rightStickValue = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Look.canceled += ctx => rightStickValue = Vector2.zero;
    }
}

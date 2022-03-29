using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool anyMenuOpen = false;
    public bool shopMenuOpen = false;

    PlayerAnimation playerAnimation;
    PlayerScript playerScript;
    PlayerSound playerSound;
    Rigidbody rb;
    GameObject pauseMenu;
    Vector3 mouseDirection;
    Vector3 moveDirection;
    Vector3 dashDirection;
    float horizontalAxis;
    float verticalAxis;
    float dashSpeed = 1000;
    float dashTime = 0;
    float maxDashTime = 0.2f;
    float dashStaminaCost = 30f;
    string healString = "Heal";
    string blockString = "Block";
    float blockCD = 3;
    public bool preventInput = false;
    public bool shield;

    public bool pathActive = false;
    float swordMaxTime = 5;
    float swordTimer;
    float pathOfPathMaxTime = 0.04f;
    float pathOfPathTimer;
    [SerializeField] GameObject pathTrailPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //Set references to other player scripts
        playerAnimation = GetComponent<PlayerAnimation>();
        playerScript = GetComponent<PlayerScript>();
        playerSound = GetComponentInChildren<PlayerSound>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //get the inputs from the keyboard/controller for movement
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        //turn those inputs into a vector that cannot have a magnitude greater than 1
        moveDirection = new Vector3(horizontalAxis, 0, verticalAxis);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);


        //If right mouse button is pressed the player should dash if they are currently able
        if (Input.GetMouseButtonDown(1))
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

        //If left mouse button is pressed they player will attack if they are currently able
        if(Input.GetMouseButtonDown(0) && !pauseMenu)
        {
            if (playerAnimation.attacking)
            {
                playerAnimation.ChainAttacks();
            }
            else if (CanInput() && playerScript.stamina > 0)
            {
                Attack();
            }
        }

        DuckAbilities();

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (anyMenuOpen)
            {
                if (shopMenuOpen)
                {
                    Shop shop = GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>();
                    shop.CloseShop();
                }
            }
            else if(!pauseMenu)
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

    void DuckAbilities()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerData.equippedAbility = healString;
        }

        if (playerData.unlockedAbilities.Contains(blockString) && Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerData.equippedAbility = blockString;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            NextAbility();
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            PreviousAbility();
        }


        if (CanInput() && Input.GetButtonDown("Jump"))
        {
            if (playerData.duckCD <= 0)
            {
                rb.velocity = Vector3.zero;
                if(playerData.equippedAbility != healString)
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
                        playerData.hasHealed = true;
                        playerData.duckCD += 1;
                        break;
                    case "Block":
                        playerData.duckCD += blockCD;
                        break;
                }
            }
        }

        if(shield && Input.GetButton("Jump"))
        {
            playerAnimation.continueBlocking = true;
            playerData.duckCD = blockCD;
        }
        else
        {
            playerAnimation.continueBlocking = false;
        }

        if (!playerData.hasHealed && playerData.duckCD > 0)
        {
            playerData.duckCD -= Time.deltaTime;
            if (playerData.equippedEmblems.Contains(emblemLibrary.magical_acceleration))
            {
                playerData.duckCD -= Time.deltaTime;
            }
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

    void Attack()
    {
        playerAnimation.attack = true;
        playerAnimation.attacking = true;
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

    //The attack point is used to determine if an attack hits. It always stays between the player and the mouse
    void AttackPointPosition()
    {
        mouseDirection = playerAnimation.mousePosition - playerAnimation.playerScreenPosition;
        mouseDirection = new Vector3(mouseDirection.x, 0.5f, mouseDirection.y);
        attackPoint.transform.position = transform.position + mouseDirection.normalized;
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
}

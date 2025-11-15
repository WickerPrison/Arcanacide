using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System;

public class PlayerMovement : MonoBehaviour
{
    //Input in inspector
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] GameObject pauseMenuPrefab;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    //Managers
    InputManager im;
    GameManager gm;
    SoundManager sm;

    //Player scripts
    PlayerAnimation playerAnimation;
    PlayerEvents playerEvents;
    PlayerScript playerScript;
    PlayerSound playerSound;
    [System.NonSerialized] public Rigidbody rb;


    //walk varibles
    [System.NonSerialized] public Vector3 moveDirection;
    [System.NonSerialized] public Vector3 lastMoveDir;
    [System.NonSerialized] public float walkSpeed = 300;
    [System.NonSerialized] public float floatSpeed = 200;
    [System.NonSerialized] public float moveSpeed = 300;

    //dash variables
    [System.NonSerialized] public Vector3 dashDirection;
    [System.NonSerialized] public bool isDashing = false;
    [NonSerialized] public float dashSpeed = 1000;
    [NonSerialized] public float dashStaminaCost = 30f;
    
    //facing direction variables
    Vector3 mouseDirection;
    [System.NonSerialized] public Vector2 rightStickValue;
    float lockOnDistance = 10;
    [System.NonSerialized] public Vector2 lookDir;
    [System.NonSerialized] public GameObject pauseMenu;
    Vector3 attackPointPos;

    //toggles
    [System.NonSerialized] public bool knockback = false;
    [System.NonSerialized] public bool canWalk = false;
    [System.NonSerialized] public bool preventInput = false;
    [System.NonSerialized] public bool lockPosition = false;
    bool lockAttackPoint = false;
    bool usingGamepad;


    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sm = gm.gameObject.GetComponent<SoundManager>();
        SetUpControls();

        playerAnimation = GetComponent<PlayerAnimation>();
        playerScript = GetComponent<PlayerScript>();
        playerSound = GetComponentInChildren<PlayerSound>();
        rb = GetComponent<Rigidbody>();
        usingGamepad = Gamepad.current != null;
        rightStickValue = Vector2.zero;
    }

    void Update()
    {
        moveDirection = new Vector3(playerData.moveDir.x, 0, playerData.moveDir.y);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

        if(moveDirection.magnitude > 0)
        {
            lastMoveDir = moveDirection.normalized;
        }

        AttackPointPosition();

        if (lockPosition)
        {
            rb.velocity = Vector3.zero;
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

        CheckForController();
    }

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
        rb.velocity = Vector3.zero;
        if (CanInput() || canWalk)
        {
            rb.velocity = new Vector3(moveDirection.x * Time.fixedDeltaTime * moveSpeed, rb.velocity.y, moveDirection.z * Time.fixedDeltaTime * moveSpeed);
        }
        //dash if the player has pressed the right mouse button
        else if (isDashing)
        {
            rb.velocity = (dashDirection * Time.fixedDeltaTime * dashSpeed);
        }
    }

    public void Dodge()
    {
        if (CanInput() && playerScript.stamina > 0 && moveDirection.magnitude > 0)
        {
            //The player dashes in whatever direction they were already moving
            float staminaCost = dashStaminaCost;
            dashDirection = moveDirection.normalized;
            if (playerData.equippedPatches.Contains(Patches.QUICKSTEP))
            {
                staminaCost /= 2;
            }

            if (playerData.equippedPatches.Contains(Patches.SHELL_COMPANY))
            {
                staminaCost *= 2;
            }

            playerScript.LoseStamina(staminaCost);
            isDashing = true;
            gameObject.layer = 8;
            playerAnimation.PlayAnimation("Dash");
            playerSound.PlaySoundEffect(PlayerSFX.DODGE, 0.5f);
        }
    }

    //The attack point is used to determine if an attack hits. It always stays between the player and the mouse
    void AttackPointPosition()
    {
        if (lockAttackPoint)
        {
            attackPoint.position = attackPointPos;
        }

        if (!CanInput() && !canWalk) return;

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

    public void LockAttackPoint()
    {
        attackPointPos = attackPoint.transform.position;
        lockAttackPoint = true;
    }

    public void UnlockAttackPoint()
    {
        lockAttackPoint = false;
    }

    void LockOn()
    {
        if (gm.enemies.Count < 1)
        {
            return;
        }
        Transform lockOnTarget = GetLockOnTarget();
        if (lockOnTarget != null)
        {
            Vector3 lockOnDirection = lockOnTarget.position - transform.position;
            lookDir = new Vector2(lockOnDirection.x, lockOnDirection.z);
        }
    }

    public Transform GetLockOnTarget()
    {
        Transform lockOnTarget = null;
        float currentDistance = lockOnDistance;
        for (int enemy = 0; enemy < gm.enemies.Count; enemy++)
        {
            if (Vector3.Distance(transform.position, gm.enemies[enemy].transform.position) < currentDistance && !gm.enemies[enemy].dying)
            {
                lockOnTarget = gm.enemies[enemy].transform;
                currentDistance = Vector3.Distance(transform.position, gm.enemies[enemy].transform.position);
            }
        }
        return lockOnTarget;
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
        if (isDashing)
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

    public IEnumerator KnockBack(float knockbackTime)
    {
        knockback = false;
        yield return new WaitForSeconds(knockbackTime);
        knockback = true;
        rb.velocity = Vector3.zero;
    }

    void CheckForController()
    {
        if(Gamepad.current == null && usingGamepad)
        {
            if (!pauseMenu && im.controlMode == ControlMode.GAMEPLAY)
            {
                PauseMenu();
            }
            usingGamepad = false;
        }
        else if(Gamepad.current != null && !usingGamepad)
        {
            usingGamepad = true;
        }
    }

    private void PlayerEvents_onPlayerStagger(object sender, System.EventArgs e)
    {
        if (!knockback)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void onEndPlayerStagger(object sender, System.EventArgs e)
    {
        preventInput = false;
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += PlayerEvents_onPlayerStagger;
        playerEvents.onEndPlayerStagger += onEndPlayerStagger;
    }


    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= PlayerEvents_onPlayerStagger;
        playerEvents.onEndPlayerStagger -= onEndPlayerStagger;
    }

    void SetUpControls()
    {
        im = gm.GetComponent<InputManager>();

        im.controls.Gameplay.Dodge.performed += ctx => Dodge();
        im.controls.Gameplay.PauseMenu.performed += ctx => PauseMenu();
        im.controls.Gameplay.Move.performed += ctx => playerData.moveDir = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Move.canceled += ctx => playerData.moveDir = Vector2.zero;
        im.controls.Gameplay.Look.performed += ctx => rightStickValue = ctx.ReadValue<Vector2>();
        im.controls.Gameplay.Look.canceled += ctx => rightStickValue = Vector2.zero;
    }
}

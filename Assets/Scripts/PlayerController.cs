using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //This script is responsible for accepting inputs from the player and performing actions 
    //that those inputs dictate

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float moveSpeed = 5;

    PlayerAnimation playerAnimation;
    PlayerScript playerScript;
    Vector3 mouseDirection;
    Vector3 moveDirection;
    Vector3 dashDirection;
    float horizontalAxis;
    float verticalAxis;
    float dashSpeed = 15;
    float dashTime = 0;
    float maxDashTime = 0.2f;
    float attackStaminaCost = 20f;
    float dashStaminaCost = 30f;

    // Start is called before the first frame update
    void Start()
    {
        //Set references to other player scripts
        playerAnimation = GetComponent<PlayerAnimation>();
        playerScript = GetComponent<PlayerScript>();
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

        //hold down left shift to sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveDirection *= 2;
        }

        //If right mouse button is pressed the player should dash if they are currently able
        if (Input.GetMouseButtonDown(1))
        {
            if (CanInput() && playerScript.stamina > 0 && moveDirection.magnitude > 0)
            {
                //They player dashes in whatever direction they were already moving
                dashDirection = moveDirection.normalized;
                dashTime = maxDashTime;
                playerScript.LoseStamina(dashStaminaCost);
                playerAnimation.DashAnimation();
            }
        }

        //If left mouse button is pressed they player will attack if they are currently able
        if(Input.GetMouseButtonDown(0))
        {
            if (CanInput() && playerScript.stamina > 0)
            {
                Attack();
            }
        }

        AttackPointPosition();
    }

    void Attack()
    {
        playerScript.LoseStamina(attackStaminaCost);
        playerAnimation.attack = true;
        playerAnimation.attacking = true;
    }

    //The attack point is used to determine if an attack hits. It always stays between the player and the mouse
    void AttackPointPosition()
    {
        mouseDirection = playerAnimation.mousePosition - playerAnimation.playerScreenPosition;
        mouseDirection = new Vector3(mouseDirection.x, 0.5f, mouseDirection.y);
        attackPoint.transform.position = transform.position + mouseDirection.normalized;
    }

    //Returns a bool that is true if the player is currently allowed to give new inputs
    bool CanInput()
    {
        if(dashTime > 0)
        {
            return false;
        }
        if (playerAnimation.attacking)
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
        transform.Translate(dashDirection * Time.fixedDeltaTime * dashSpeed);
        dashTime -= Time.fixedDeltaTime;
    }

    //FixedUpdate is similar to Update but should always be used when dealing with physics
    private void FixedUpdate()
    {
        if(moveDirection.magnitude > 0 && !playerAnimation.attacking)
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
            transform.Translate(moveDirection * Time.fixedDeltaTime * moveSpeed);
        }
        //dash if the player has pressed the right mouse button
        else if(!playerAnimation.attacking)
        {
            Dash();
        }
    }
}

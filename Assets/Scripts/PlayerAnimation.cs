using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    //This script is responsible for managing the player's animations
    //The player has a front and a back animator. At any moment one will be hidden offscreen, but will still exist
    //Animations are always called on both animators, even though one is unseen. This allows us to smoothly switch between
    //the two without worrying about lining up the timing or animations being cut short

    public Animator parryAnimator;
    public Vector3 mousePosition;
    public Vector3 playerFeetPosition;
    public Vector3 playerScreenPosition;
    public bool walk;
    public bool attack;
    public bool attacking;
    public bool continueBlocking;
    public bool parryWindow = false;
    public bool isParrying = false;
    Smear smear;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] ParticleSystem bodyMagic;
    [SerializeField] ParticleSystem frontSwordMagic;
    [SerializeField] ParticleSystem backSwordMagic;
    [SerializeField] Camera cam;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] PlayerController playerController;
    PlayerSound playerSound;

    Vector3 away = new Vector3(100, 100, 100);
    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    float initalScaleX;
    float frontOffset;
    float backOffset;

    private void Start()
    {
        playerSound = gameObject.GetComponentInChildren<PlayerSound>();
        frontAnimatorPosition = frontAnimator.transform.localPosition;
        backAnimatorPosition = backAnimator.transform.localPosition;
        initalScaleX = frontAnimator.transform.localScale.x;
        frontOffset = frontAnimator.transform.localPosition.x;
        backOffset = backAnimator.transform.localPosition.x;
        smear = GetComponentInChildren<Smear>();

        if (Gamepad.current == null)
        {
            FaceMouse();
        }
        else
        {
            FaceJoystick();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        StaminaUpdate();

        //While attacking the player won't change what direction he is facing. Otherwise he faces the mouse
        if (playerController.CanInput())
        {
            if(Gamepad.current == null)
            {
                FaceMouse();
            }
            else
            {
                FaceJoystick();
            }
        }

        if (walk)
        {
            frontAnimator.SetBool("Walk", true);
            backAnimator.SetBool("Walk", true);
        }
        else
        {
            frontAnimator.SetBool("Walk", false);
            backAnimator.SetBool("Walk", false);
        }

        if (attack)
        {
            attack = false;
            frontAnimator.Play("Attack");
            backAnimator.Play("Attack");
        }

        frontAnimator.SetBool("ContinueBlocking", continueBlocking);
        backAnimator.SetBool("ContinueBlocking", continueBlocking);
    }

    void StaminaUpdate()
    {
        frontAnimator.SetFloat("Stamina", playerScript.stamina);
        backAnimator.SetFloat("Stamina", playerScript.stamina);
    }

    public void PlayStagger()
    {
        attacking = false;
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }

    public void PlayIdle()
    {
        frontAnimator.Play("Idle");
        backAnimator.Play("Idle");
    }

    public void StopBlocking()
    {
        frontAnimator.Play("StopBlocking");
        backAnimator.Play("StopBlocking");
    }

    public void Shield()
    {
        frontAnimator.Play("Block");
        backAnimator.Play("Block");
    }

    public void Shove()
    {
        frontAnimator.Play("Shove");
        backAnimator.Play("Shove");
    }

    public void DashAnimation()
    {
        frontAnimator.Play("Dash");
        backAnimator.Play("Dash");
    }

    public void ChainAttacks()
    {
        frontAnimator.SetBool("Attacks", true);
        backAnimator.SetBool("Attacks", true);
    }

    public void EndChain()
    {
        frontAnimator.SetBool("Attacks", false);
        backAnimator.SetBool("Attacks", false);
    }

    public void ParryAnimation()
    {
        parryAnimator.Play("Parry");
    }

    public void StartBodyMagic()
    {
        bodyMagic.Play();
        playerSound.Magic();
    }

    public void EndBodyMagic()
    {
        bodyMagic.Stop();
        playerSound.StopSoundEffect();
    }

    public void StartSwordMagic()
    {
        frontSwordMagic.Play();
        backSwordMagic.Play();
        playerSound.Magic();
    }

    public void EndSwordMagic()
    {
        frontSwordMagic.Stop();
        backSwordMagic.Stop();
        playerSound.StopSoundEffect();
    }

    void FaceJoystick()
    {
        if(playerController.lookDir.x > 0)
        {
            if(playerController.lookDir.y > 0)
            {
                BackRight();
            }
            else
            {
                FrontRight();
            }
        }
        else
        {
            if(playerController.lookDir.y > 0)
            {
                BackLeft();
            }
            else
            {
                FrontLeft();
            }
        }
    }

    void FaceMouse()
    {
        //find the mouse position in screen coordinates
        mousePosition = Mouse.current.position.ReadValue();
        //find the position of the player's feet and convert it to screen coordinates
        playerFeetPosition = new Vector3(transform.position.x, 0, transform.position.z);
        playerScreenPosition = cam.WorldToScreenPoint(playerFeetPosition);

        if (mousePosition.y < playerScreenPosition.y)
        {
            if (mousePosition.x > playerScreenPosition.x)
            {
                FrontRight();
            }
            else
            {
                FrontLeft();
            }
        }
        else
        {
            if (mousePosition.x > playerScreenPosition.x)
            {
                BackRight();
            }
            else
            {
                BackLeft();
            }
        }
    }

    void FrontRight()
    {
        smear.facingDirection = 0;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void FrontLeft()
    {
        smear.facingDirection = 1;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void BackLeft()
    {
        smear.facingDirection = 2;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void BackRight()
    {
        smear.facingDirection = 3;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }
}

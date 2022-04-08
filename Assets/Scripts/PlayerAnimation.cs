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

    public Animator smearAnimator;
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
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] ParticleSystem bodyMagic;
    [SerializeField] ParticleSystem frontSwordMagic;
    [SerializeField] ParticleSystem backSwordMagic;
    public ParticleSystem frontSmear;
    public ParticleSystem backSmear;
    [SerializeField] Camera cam;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] PlayerController playerController;
    PlayerSound playerSound;

    Vector3 frontSmearScale;
    Vector3 frontSmearRotation;
    Vector3 frontSmearPosition;
    Vector3 backSmearScale;
    Vector3 backSmearRotation;
    Vector3 backSmearPosition;
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
        frontSmearScale = frontSmear.transform.localScale;
        frontSmearRotation = new Vector3(90, -20, 0);
        frontSmearPosition = new Vector3(0.32f, 0, -0.32f);
        backSmearScale = backSmear.transform.localScale;
        backSmearRotation = new Vector3(-90, 70, 0);
        backSmearPosition = new Vector3(0.32f, 0, 0.32f);
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
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }

    public void UseDuck(string abilityName)
    {
        frontAnimator.Play(abilityName);
        backAnimator.Play(abilityName);
    }

    public void StaggerUpdate(float stagger)
    {
        frontAnimator.SetFloat("Stagger", stagger);
        backAnimator.SetFloat("Stagger", stagger);
    }

    public void DashAnimation()
    {
        frontAnimator.Play("Dash");
        backAnimator.Play("Dash");
    }

    public void particleSmear()
    {
        frontSmear.Play();
        backSmear.Play();
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
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
        frontSmear.transform.localScale = frontSmearScale;
        frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, frontSmearRotation.y, frontSmearRotation.z);
        frontSmear.transform.localPosition = frontSmearPosition;
        backSmear.transform.position = away;
    }

    void FrontLeft()
    {
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
        frontSmear.transform.localScale = new Vector3(-frontSmearScale.x, frontSmearScale.y, frontSmearScale.z);
        frontSmear.transform.localRotation = Quaternion.Euler(frontSmearRotation.x, -frontSmearRotation.y, frontSmearRotation.z);
        frontSmear.transform.localPosition = new Vector3(-frontSmearPosition.x, frontSmearPosition.y, frontSmearPosition.z);
        backSmear.transform.position = away;

    }

    void BackLeft()
    {
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
        frontSmear.transform.position = away;
        backSmear.transform.localScale = backSmearScale;
        backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, -backSmearRotation.y, backSmearRotation.z);
        backSmear.transform.localPosition = new Vector3(-backSmearPosition.x, backSmearPosition.y, backSmearPosition.z);
    }

    void BackRight()
    {
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
        frontSmear.transform.position = away;
        backSmear.transform.localScale = new Vector3(-backSmearScale.x, backSmearScale.y, backSmearScale.z);
        backSmear.transform.localRotation = Quaternion.Euler(backSmearRotation.x, backSmearRotation.y, backSmearRotation.z);
        backSmear.transform.localPosition = backSmearPosition;
    }
}

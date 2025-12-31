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

    //public Animator parryAnimator;
    [System.NonSerialized] public Vector3 mousePosition;
    [System.NonSerialized] public Vector3 playerRotationPoint;
    [System.NonSerialized] public Vector3 playerScreenPosition;
    [System.NonSerialized] public bool walk;
    [System.NonSerialized] public bool attacking;
    [System.NonSerialized] public bool continueBlocking;
    [System.NonSerialized] public bool parryWindow = false;
    [System.NonSerialized] public bool isParrying = false;
    [System.NonSerialized] public bool facingFront;
    Smear smear;
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] PlayerAttackHitEvents attackHitEvents;
    [SerializeField] ParticleSystem bodyMagic;
    int bodyMagicSources = 0;
    Camera cam;
    [SerializeField] PlayerScript playerScript;
    [SerializeField] PlayerMovement playerController;
    [SerializeField] float rotationPointY;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] PlayerData playerData;
    [SerializeField] AttackProfiles swordHeavyProfile;
    [SerializeField] AttackProfiles clawHeavyProfile;

    Vector3 away = new Vector3(100, 100, 100);
    Vector3 frontAnimatorPosition;
    Vector3 backAnimatorPosition;
    float initalScaleX;
    float frontOffset;
    float backOffset;
    public ParticleSystem shoveVFX;

    public int facingDirection;
    PlayerEvents playerEvents;

    private void Awake()
    {
        playerEvents = GetComponent<PlayerEvents>();
    }

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
        //Debug.Log(attacking);
        StaminaUpdate();

        //While attacking the player won't change what direction he is facing. Otherwise he faces the mouse
        if (playerController.CanInput() || playerController.canWalk)
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

        frontAnimator.SetBool("ContinueBlocking", continueBlocking);
        backAnimator.SetBool("ContinueBlocking", continueBlocking);
    }

    public void PlayAnimation(string animationName)
    {
        frontAnimator.Play(animationName);
        backAnimator.Play(animationName);
    }

    public void PlayAnimation(string animationName, float time, int layer = -1)
    {
        frontAnimator.Play(animationName, layer, time);
        backAnimator.Play(animationName, layer, time);
    }

    public void StaminaUpdate()
    {
        frontAnimator.SetFloat("Stamina", playerScript.stamina);
        backAnimator.SetFloat("Stamina", playerScript.stamina);
    }


    public void PlayStagger(object sender, System.EventArgs e)
    {
        attacking = false;
        frontAnimator.Play("Stagger");
        backAnimator.Play("Stagger");
    }


    public void HealAnimation()
    {
        if (!playerData.hasHealthGem) return;
        if(playerController.CanInput() && playerData.healCharges >= 0)
        {
            //frontAnimator.SetLayerWeight(1, 1);
            //backAnimator.SetLayerWeight(1, 1);
            frontAnimator.Play("Heal");
            backAnimator.Play("Heal");
        }
    }

    public void ChainAttacks()
    {
        frontAnimator.SetBool("Attacks", true);
        backAnimator.SetBool("Attacks", true);
    }

    public void Combo()
    {
        frontAnimator.SetBool("Combo", true);
        backAnimator.SetBool("Combo", true);
    }

    public void EndChain()
    {
        frontAnimator.SetBool("Attacks", false);
        backAnimator.SetBool("Attacks", false);
        frontAnimator.SetBool("Combo", false);
        backAnimator.SetBool("Combo", false);
    }

    public void SetBool(string name, bool value)
    {
        frontAnimator.SetBool(name, value);
        backAnimator.SetBool(name, value);
    }

    public void StartBodyMagic()
    {
        bodyMagicSources += 1;
        bodyMagic.Play();
    }

    public void EndBodyMagic()
    {
        bodyMagicSources -= 1;
        if(bodyMagicSources <= 0)
        {
            bodyMagic.Stop();
        }
    }

    public float EndSwordHeavy()
    {
        return attackHitEvents.EndCharge(swordHeavyProfile.maxChargeTime);
    }

    public float EndClawHeavy()
    {
        return attackHitEvents.EndCharge(clawHeavyProfile.maxChargeTime);
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
        //find the position of the player's rotation point and convert it to screen coordinates
        playerRotationPoint = new Vector3(transform.position.x, rotationPointY, transform.position.z);
        playerScreenPosition = cam.WorldToScreenPoint(playerRotationPoint);

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
        facingFront = true;
        facingDirection = 0;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void FrontLeft()
    {
        facingFront = true;
        facingDirection = 1;
        backAnimator.transform.localPosition = away;
        frontAnimator.transform.localPosition = frontAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void BackLeft()
    {
        facingFront = false;
        facingDirection = 2;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(-backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    void BackRight()
    {
        facingFront = false;
        facingDirection = 3;
        frontAnimator.transform.localPosition = away;
        backAnimator.transform.localPosition = backAnimatorPosition;
        frontAnimator.transform.localScale = new Vector3(-initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        frontAnimator.transform.localPosition = new Vector3(-frontOffset, frontAnimator.transform.localPosition.y, frontAnimator.transform.localPosition.z);
        backAnimator.transform.localScale = new Vector3(initalScaleX, frontAnimator.transform.localScale.y, frontAnimator.transform.localScale.z);
        backAnimator.transform.localPosition = new Vector3(backOffset, backAnimator.transform.localPosition.y, backAnimator.transform.localPosition.z);
    }

    private void OnEnable()
    {
        playerEvents.onPlayerStagger += PlayStagger;
    }

    private void OnDisable()
    {
        playerEvents.onPlayerStagger -= PlayStagger;
    }
}

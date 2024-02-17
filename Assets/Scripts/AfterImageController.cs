using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageController : MonoBehaviour
{
    [SerializeField] GameObject afterimagePrefab;
    [SerializeField] Transform frontAnimator;
    [SerializeField] Transform backAnimator;
    [SerializeField] PlayerData playerData;
    PlayerEvents playerEvents;
    PlayerAnimation playerAnimation;
    PlayerMovement playerMovement;
    IEnumerator coroutine;
    WaitForSeconds imageRate;
    bool effectPlaying = false;
    float maxTime = 0.2f;
    float timer;
    float afterImageRate = 0.02f;
    int backstepNum = 0;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        imageRate = new WaitForSeconds(afterImageRate);
    }

    private void onDashStart(object sender, System.EventArgs e)
    {
        if (!effectPlaying)
        {
            backstepNum = 0;
            coroutine = PlaceAfterImages();
            StartCoroutine(coroutine);
        }
    }

    private void onBackstepStart(object sender, System.EventArgs e)
    {
        if (!effectPlaying)
        {
            backstepNum = playerEvents.backstepInt;
            coroutine = PlaceAfterImages();
            StartCoroutine (coroutine);
        }
    }

    private void onDashEnd(object sender, System.EventArgs e)
    {
        if (effectPlaying)
        {
            effectPlaying = false;
            StopCoroutine(coroutine);
        }
    }

    IEnumerator PlaceAfterImages()
    {
        effectPlaying = true;
        timer = maxTime;

        while(timer > 0)
        {
            if (!playerMovement.isDashing)
            {
                timer = 0;
                
            }

            timer -= Time.deltaTime;

            AfterImage afterImage = Instantiate(afterimagePrefab).GetComponent<AfterImage>();
            Transform playerTransform;
            if (playerAnimation.facingFront)
            {
                playerTransform = frontAnimator;
            }
            else
            {
                playerTransform = backAnimator;
            }

            int weaponInt;
            if(backstepNum != 0)
            {
                weaponInt = backstepNum;
            }
            else
            {
                weaponInt = playerData.currentWeapon;
            }
            afterImage.PlaceAfterImage(weaponInt, playerAnimation.facingFront, playerTransform);

            yield return imageRate;
        }

        effectPlaying = false;
    }

    private void OnEnable()
    {
        playerEvents.onDashStart += onDashStart;
        playerEvents.onDashEnd += onDashEnd;
        playerEvents.onBackstepStart += onBackstepStart;
    }


    private void OnDisable()
    {
        playerEvents.onDashStart -= onDashStart;
        playerEvents.onDashEnd -= onDashEnd;
    }

}

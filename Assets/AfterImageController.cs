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
    WaitForSeconds imageRate;
    bool effectPlaying = false;
    float maxTime = 0.2f;
    float timer;
    float afterImageRate = 0.02f;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        imageRate = new WaitForSeconds(afterImageRate);
    }

    private void onDashStart(object sender, System.EventArgs e)
    {
        if(!effectPlaying) StartCoroutine(PlaceAfterImages());
    }

    IEnumerator PlaceAfterImages()
    {
        effectPlaying = true;
        timer = maxTime;

        while(timer > 0)
        {
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

            afterImage.PlaceAfterImage(playerData.currentWeapon, playerAnimation.facingFront, playerTransform);

            yield return imageRate;
        }

        effectPlaying = false;
    }

    private void OnEnable()
    {
        playerEvents.onDashStart += onDashStart;
    }

    private void OnDisable()
    {
        playerEvents.onDashStart -= onDashStart;
    }

}

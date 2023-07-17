using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    [SerializeField] SpriteRenderer frontSword;
    [SerializeField] SpriteRenderer frontLantern;
    [SerializeField] SpriteRenderer frontKnife;
    [SerializeField] SpriteRenderer frontKnife2;
    [SerializeField] SpriteRenderer frontClaws;
    [SerializeField] SpriteRenderer frontClaws2;
    [SerializeField] SpriteRenderer backSword;
    [SerializeField] SpriteRenderer backLantern;
    [SerializeField] SpriteRenderer backKnife;
    [SerializeField] SpriteRenderer backKnife2;
    [SerializeField] SpriteRenderer backClaws;
    [SerializeField] SpriteRenderer backClaws2;
    Dictionary<int, string> animationDict = new Dictionary<int, string>()
    {
        {0, "SwordDash" },
        {1, "LanternDash" },
        {2, "KnifeDash" },
        {3, "ClawsDash" }
    };
    SpriteRenderer[] renderers;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    float lifeTime = 0.2f;
    float timer;


    public void PlaceAfterImage(int weapon, bool faceFront, Transform playerTransform)
    {
        transform.position = playerTransform.position;
        if (faceFront)
        {
            backAnimator.gameObject.SetActive(false);
            switch (weapon)
            {
                case 0:
                    frontSword.gameObject.SetActive(true);
                    break;
                case 1:
                    frontLantern.gameObject.SetActive(true);
                    break;
                case 2:
                    frontKnife.gameObject.SetActive(true);
                    frontKnife2.gameObject.SetActive(true);
                    break;
                case 3:
                    frontClaws.gameObject.SetActive(true);
                    frontClaws2.gameObject.SetActive(true);
                    break;
            }
            SetupAnimator(frontAnimator, weapon, playerTransform);
        }
        else
        {
            frontAnimator.gameObject.SetActive(false);
            switch (weapon)
            {
                case 0:
                    backSword.gameObject.SetActive(true);
                    break;
                case 1:
                    backLantern.gameObject.SetActive(true);
                    break;
                case 2:
                    backKnife.gameObject.SetActive(true);
                    backKnife2.gameObject.SetActive(true);
                    break;
                case 3:
                    backClaws.gameObject.SetActive(true);
                    backClaws2.gameObject.SetActive(true);
                    break;
            }
            SetupAnimator(backAnimator, weapon, playerTransform);
        }
    }

    void SetupAnimator(Animator animator, int weapon, Transform playerTransform)
    {
        animator.gameObject.SetActive(true);
        animator.transform.localScale = playerTransform.localScale;
        animator.Play(animationDict[weapon]);
        renderers = animator.gameObject.GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(Fadeaway());
    }

    IEnumerator Fadeaway()
    {
        timer = lifeTime;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / lifeTime;

            foreach(SpriteRenderer sprite in renderers)
            {
                sprite.color = new Color(1, 1, 1, alpha);
            }

            yield return endOfFrame;
        }

        Destroy(gameObject);
    }
}

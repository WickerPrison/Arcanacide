using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AfterimageWeapon
{
    SWORD, LANTERN, KNIFE, CLAWS
}

public class AfterImage : MonoBehaviour
{
    [SerializeField] Animator frontAnimator;
    [SerializeField] Animator backAnimator;
    SpriteRenderer[] renderers;
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    float lifeTime = 0.2f;
    float timer;


    public void PlaceAfterImage(AfterimageWeapon weapon, bool faceFront, Transform playerTransform)
    {
        transform.position = playerTransform.position;
        if (faceFront)
        {
            backAnimator.gameObject.SetActive(false);
            SetupAnimator(frontAnimator, weapon, playerTransform);
        }
        else
        {
            frontAnimator.gameObject.SetActive(false);
            SetupAnimator(backAnimator, weapon, playerTransform);
        }
    }

    void SetupAnimator(Animator animator, AfterimageWeapon weapon, Transform playerTransform)
    {
        animator.gameObject.SetActive(true);
        animator.transform.localScale = playerTransform.localScale;
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

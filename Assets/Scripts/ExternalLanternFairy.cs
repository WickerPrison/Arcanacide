using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalLanternFairy : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] lanternFairies;
    Transform movePoint;
    PlayerEvents playerEvents;
    PlayerAnimation playerAnimation;
    SpriteRenderer spriteRenderer;
    ParticleSystem trail;
    [System.NonSerialized] public bool isInLantern = true;
    bool returning = false;
    float speed = 30;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    private void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
        movePoint = GetComponentsInChildren<Transform>()[1];
        movePoint.parent = transform.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (returning)
        {
            if (playerAnimation.facingFront)
            {
                movePoint.position = lanternFairies[0].transform.position;
            }
            else
            {
                movePoint.position = lanternFairies[1].transform.position;
            }
        }

        Vector3 direction = movePoint.localPosition - transform.localPosition;
        transform.localPosition += speed * Time.fixedDeltaTime * direction.normalized;

        if (Vector3.Distance(transform.localPosition, movePoint.localPosition) < speed * Time.fixedDeltaTime)
        {
            spriteRenderer.enabled = false;
            trail.Stop();
            trail.Clear();
            if (returning)
            {
                returning = false;
                ToggleSprites(true);
            }
        }
    }

    private void StartAxeSpecial(object sender, System.EventArgs e)
    {
        if (playerAnimation.facingFront)
        {
            transform.position = lanternFairies[0].transform.position;
        }
        else
        {
            transform.position = lanternFairies[1].transform.position;
        }

        spriteRenderer.enabled = true;
        trail.Play();
        ToggleSprites(false);

        movePoint.position = transform.position + Vector3.up * 10;
    }

    public void Return(Vector3 position)
    {
        transform.position = position;
        spriteRenderer.enabled = true;
        trail.Play();
        returning = true;
    }

    public void ToggleSprites(bool spritesOn)
    {
        isInLantern = spritesOn;
        foreach(SpriteRenderer sprite in lanternFairies)
        {
            sprite.enabled = spritesOn;
        }
    }

    private void OnEnable()
    {
        playerEvents.onAxeSpecial += StartAxeSpecial;
    }
}

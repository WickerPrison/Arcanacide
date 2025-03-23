using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalLanternFairy : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] lanternFairies;
    [SerializeField] Transform attackPoint;
    Transform movePoint;
    PlayerEvents playerEvents;
    PlayerAnimation playerAnimation;
    SpriteRenderer spriteRenderer;
    ParticleSystem trail;
    [System.NonSerialized] public bool isInLantern = true;
    bool isDoingSpecial = false;
    bool returning = false;
    float speed = 30;
    Vector3 offset = new Vector3(0, 1, 0);
    enum ComboState
    {
        OFF, START, FIRE_RAIN
    }
    ComboState comboState;

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
            switch (comboState)
            {
                case ComboState.START:
                    transform.localPosition = movePoint.localPosition;
                    comboState = ComboState.FIRE_RAIN;
                    playerEvents.StartFireRain(transform.position);
                    break;
                case ComboState.FIRE_RAIN:
                    break;
                case ComboState.OFF:
                    spriteRenderer.enabled = false;
                    trail.Stop();
                    trail.Clear();
                    if (returning)
                    {
                        isDoingSpecial = false;
                        returning = false;
                        ToggleSprites(true);
                    }
                    break;
            }
        }
    }

    private void StartAxeSpecial(object sender, System.EventArgs e)
    {
        isDoingSpecial = true;
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

    private void StartLanternCombo(object sender, System.EventArgs e)
    {
        comboState = ComboState.START;
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

    public void EndLanternCombo()
    {
        comboState = ComboState.OFF;
        Vector3 destination;
        if (playerAnimation.facingFront)
        {
            destination = lanternFairies[0].transform.position;
        }
        else
        {
            destination = lanternFairies[1].transform.position;
        }
        Return(destination);
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

    private void onPlayerStagger(object sender, System.EventArgs e)
    {
        if (isInLantern || isDoingSpecial || comboState != ComboState.OFF) return;

        Vector3 destination;
        if (playerAnimation.facingFront)
        {
            destination = lanternFairies[0].transform.position;
        }
        else
        {
            destination = lanternFairies[1].transform.position;
        }
        Return(destination);
    }

    private void OnEnable()
    {
        playerEvents.onAxeSpecial += StartAxeSpecial;
        playerEvents.onLanternCombo += StartLanternCombo;
        playerEvents.onPlayerStagger += onPlayerStagger;
    }

    private void OnDisable()
    {
        playerEvents.onAxeSpecial -= StartAxeSpecial;
        playerEvents.onLanternCombo -= StartLanternCombo;
        playerEvents.onPlayerStagger -= onPlayerStagger;
    }
}

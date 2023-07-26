using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

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
    bool isOrbiting = false;
    Vector3 offset = new Vector3(0, 1, 0);
    Vector3 center;
    float angle;
    float rotationSpeed = 580;

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

        if (isOrbiting)
        {
            UpdatePosition();
            return;
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
                isDoingSpecial = false;
                returning = false;
                ToggleSprites(true);
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
        angle = 0;
        isOrbiting = true;
    }

    private void EndLanternCombo(object sender, System.EventArgs e)
    {
        isOrbiting = false;
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

    void UpdatePosition()
    {
        center = playerAnimation.transform.position + offset;
        angle += rotationSpeed * Time.fixedDeltaTime;
        if (angle > 360) angle -= 360;

        Vector3 oldDirection = attackPoint.position - playerAnimation.transform.position;
        transform.position = center + RotateDirection(oldDirection, angle).normalized * 1.5f;
    }

    Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.z;
        newDirection.z = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.z;
        return newDirection;
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
        if (isInLantern || isDoingSpecial) return;
        isOrbiting = false;

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
        playerEvents.onEndLanternCombo += EndLanternCombo;
    }

    private void OnDisable()
    {
        playerEvents.onAxeSpecial -= StartAxeSpecial;
        playerEvents.onLanternCombo -= StartLanternCombo;
        playerEvents.onPlayerStagger -= onPlayerStagger;
        playerEvents.onEndLanternCombo -= EndLanternCombo;
    }
}

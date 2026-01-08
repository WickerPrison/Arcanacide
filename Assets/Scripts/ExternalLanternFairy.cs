using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalLanternFairy : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] lanternFairies;
    [SerializeField] Transform attackPoint;
    [SerializeField] PlayerData playerData;
    Transform movePoint;
    PlayerEvents playerEvents;
    PlayerAnimation playerAnimation;
    PlayerScript playerScript;
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
        playerScript = GetComponentInParent<PlayerScript>();
        movePoint = GetComponentsInChildren<Transform>()[1];
        movePoint.parent = transform.parent;
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (returning)
        {
            movePoint.position = GetInternalLanternPosition();
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
                        if (playerScript.testingEvents != null) playerScript.testingEvents.FaerieReturn();
                    }
                    break;
            }
        }
    }

    private void StartLanternSpecial(object sender, AttackProfiles attackProfile)
    {
        isDoingSpecial = true;
        transform.position = GetInternalLanternPosition();
        spriteRenderer.enabled = true;
        trail.Play();
        ToggleSprites(false);

        movePoint.position = transform.position + Vector3.up * 10;
    }

    private void StartLanternCombo(object sender, System.EventArgs e)
    {
        comboState = ComboState.START;
        transform.position = GetInternalLanternPosition();
        spriteRenderer.enabled = true;
        trail.Play();
        ToggleSprites(false);
        movePoint.position = transform.position + Vector3.up * 10;
    }

    public void EndLanternCombo()
    {
        comboState = ComboState.OFF;
        Return(GetInternalLanternPosition());
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

        Return(GetInternalLanternPosition());
    }

    public Vector3 GetInternalLanternPosition()
    {
        return (playerAnimation.facingFront, playerData.equippedElements[1]) switch
        {
            (true, WeaponElement.FIRE) => lanternFairies[0].transform.position,
            (false, WeaponElement.FIRE) => lanternFairies[1].transform.position,
            (true, WeaponElement.ELECTRICITY) => lanternFairies[2].transform.position,
            (false, WeaponElement.ELECTRICITY) => lanternFairies[3].transform.position,
        };
    }

    private void OnEnable()
    {
        playerEvents.onLanternSpecial += StartLanternSpecial;
        playerEvents.onLanternCombo += StartLanternCombo;
        playerEvents.onPlayerStagger += onPlayerStagger;
    }

    private void OnDisable()
    {
        playerEvents.onLanternSpecial -= StartLanternSpecial;
        playerEvents.onLanternCombo -= StartLanternCombo;
        playerEvents.onPlayerStagger -= onPlayerStagger;
    }
}

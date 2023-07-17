using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VFXmanager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem dodgeVFX;
    [SerializeField] ParticleSystem impactVFX;
    [SerializeField] ParticleSystem bloodVFX;
    [SerializeField] SpriteRenderer parryPulse1;
    [SerializeField] SpriteRenderer parryPulse2;
    float parryPulseDuration = 0.2f;
    float parryPulseTimer;
    float parryPulseFadeaway = 0.1f;
    float parryPulseDistance = 2f;
    float parryPulseSpeed;
    Vector3 parryPulsePosition1;
    Vector3 parryPulsePosition2;
    Color tempColor;

    [SerializeField] ParticleSystem clawSpecialVFX;

    [SerializeField] SpriteRenderer mirrorCloak;
    PlayerEvents playerEvents;
    float mirrorCloakWarmup = 1;
    float mirrorCloakTimer = 0;
    float mirrorCloakRatio;
    float mirrorCloakOffset;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    private void Start()
    {
        parryPulsePosition1 = parryPulse1.transform.localPosition;
        parryPulsePosition2 = parryPulse2.transform.localPosition;
        tempColor = parryPulse1.color;
        tempColor.a = 0;
        parryPulse1.color = tempColor;
        parryPulse2.color = tempColor;
        if (playerData.clawSpecialOn) clawSpecialVFX.Play();
        mirrorCloak.enabled = playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak);
    }

    private void onDashStart(object sender, System.EventArgs e)
    {
        dodgeVFX.Play();
    }

    private void onAttackImpact(object sender, System.EventArgs e)
    {
        impactVFX.Play();
    }

    private void onTakeDamage(object sender, System.EventArgs e)
    {
        bloodVFX.Play();
    }

    private void OnClawSpecial(object sender, System.EventArgs e)
    {
        clawSpecialVFX.Play();
    }

    private void onMeleeParry(object sender, System.EventArgs e)
    {
        parryPulse1.transform.localPosition = parryPulsePosition1;
        parryPulse2.transform.localPosition = parryPulsePosition2;
        parryPulseTimer = parryPulseDuration;
        parryPulseSpeed = parryPulseDistance / parryPulseDuration;
        tempColor = parryPulse1.color;
        tempColor.a = 1;
        parryPulse1.color = tempColor;
        parryPulse2.color = tempColor;
        StartCoroutine(ParryPulse());
    }

    IEnumerator ParryPulse()
    {
        while(parryPulseTimer > 0)
        {
            float ratio = parryPulseTimer / parryPulseDuration;
            parryPulse1.transform.localPosition += Time.deltaTime * parryPulseSpeed * ratio * Vector3.right;
            parryPulse2.transform.localPosition -= Time.deltaTime * parryPulseSpeed * ratio * Vector3.right;
            parryPulseTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        parryPulseTimer = parryPulseFadeaway;
        while(parryPulseTimer > 0)
        {
            tempColor.a = Mathf.Lerp(0,1, parryPulseTimer / parryPulseFadeaway);
            parryPulse1.color = tempColor;
            parryPulse2.color = tempColor;
            parryPulseTimer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tempColor.a = 0;
        parryPulse1.color = tempColor;
        parryPulse2.color = tempColor;
    }

    private void OnEndClawSpecial(object sender, System.EventArgs e)
    {
        clawSpecialVFX.Stop();
    }
    private void onStartMirrorCloak(object sender, System.EventArgs e)
    {
        mirrorCloak.enabled = true;
        StartCoroutine(StartMirrorCloak());
    }

    IEnumerator StartMirrorCloak()
    {
        mirrorCloakTimer = 0;
        while(mirrorCloakTimer < mirrorCloakWarmup)
        {
            mirrorCloakTimer += Time.deltaTime;
            mirrorCloakRatio = mirrorCloakTimer / mirrorCloakWarmup;
            mirrorCloakOffset = Mathf.Lerp(-1f, -0.1f, mirrorCloakRatio);
            mirrorCloak.material.SetFloat("_RadialOffset", mirrorCloakOffset);
            yield return new WaitForEndOfFrame();
        }
    }

    private void onEndMirrorCloak(object sender, System.EventArgs e)
    {
        mirrorCloak.enabled = false;
    }

    private void OnEnable()
    {
        playerEvents.onDashStart += onDashStart;
        playerEvents.onAttackImpact += onAttackImpact;
        playerEvents.onTakeDamage += onTakeDamage;
        playerEvents.onClawSpecial += OnClawSpecial;
        playerEvents.onEndClawSpecial += OnEndClawSpecial;
        playerEvents.onStartMirrorCloak += onStartMirrorCloak;
        playerEvents.onEndMirrorCloak += onEndMirrorCloak;
        playerEvents.onMeleeParry += onMeleeParry;
    }


    private void OnDisable()
    {
        playerEvents.onDashStart -= onDashStart;
        playerEvents.onAttackImpact -= onAttackImpact;
        playerEvents.onTakeDamage -= onTakeDamage;
        playerEvents.onClawSpecial -= OnClawSpecial;
        playerEvents.onEndClawSpecial -= OnEndClawSpecial;
        playerEvents.onStartMirrorCloak -= onStartMirrorCloak;
        playerEvents.onEndMirrorCloak -= onEndMirrorCloak;
        playerEvents.onMeleeParry -= onMeleeParry;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXmanager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
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
        if(playerData.clawSpecialOn) clawSpecialVFX.Play();
        mirrorCloak.enabled = playerData.equippedEmblems.Contains(emblemLibrary.mirror_cloak);
    }

    private void OnClawSpecial(object sender, System.EventArgs e)
    {
        clawSpecialVFX.Play();
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
        playerEvents.onClawSpecial += OnClawSpecial;
        playerEvents.onEndClawSpecial += OnEndClawSpecial;
        playerEvents.onStartMirrorCloak += onStartMirrorCloak;
        playerEvents.onEndMirrorCloak += onEndMirrorCloak;
    }

    private void OnDisable()
    {
        playerEvents.onClawSpecial -= OnClawSpecial;
        playerEvents.onEndClawSpecial -= OnEndClawSpecial;
        playerEvents.onStartMirrorCloak -= onStartMirrorCloak;
        playerEvents.onEndMirrorCloak -= onEndMirrorCloak;
    }
}

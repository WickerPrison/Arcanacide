using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXmanager : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] ParticleSystem clawSpecialVFX;
    PlayerEvents playerEvents;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    private void Start()
    {
        if(playerData.clawSpecialOn) clawSpecialVFX.Play();
    }

    private void OnClawSpecial(object sender, System.EventArgs e)
    {
        clawSpecialVFX.Play();
    }

    private void OnEndClawSpecial(object sender, System.EventArgs e)
    {
        clawSpecialVFX.Stop();
    }

    private void OnEnable()
    {
        playerEvents.onClawSpecial += OnClawSpecial;
        playerEvents.onEndClawSpecial += OnEndClawSpecial;
    }

    private void OnDisable()
    {
        playerEvents.onClawSpecial -= OnClawSpecial;
        playerEvents.onEndClawSpecial -= OnEndClawSpecial;
    }
}

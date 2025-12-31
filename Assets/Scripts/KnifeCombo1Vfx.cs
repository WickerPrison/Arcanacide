using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCombo1Vfx : MonoBehaviour
{
    [SerializeField] bool isFront;
    [SerializeField] ParticleSystem zapVfx;
    [SerializeField] ParticleSystem icePoofVfx;
    [SerializeField] PlayerData playerData;
    PlayerEvents playerEvents;

    private void Awake()
    {
        playerEvents = GetComponentInParent<PlayerEvents>();
    }

    private void OnEnable()
    {
        playerEvents.onKnifeCombo1Vfx += PlayerEvents_onKnifeCombo1Vfx;
    }

    private void OnDisable()
    {
        playerEvents.onKnifeCombo1Vfx -= PlayerEvents_onKnifeCombo1Vfx;
    }

    private void PlayerEvents_onKnifeCombo1Vfx(object sender, (Vector3, bool) inputs)
    {
        if (inputs.Item2 != isFront) return;
        Vector3 direction = inputs.Item1;
        transform.rotation = Quaternion.LookRotation(direction);

        switch (playerData.equippedElements[2])
        {
            case WeaponElement.ELECTRICITY:
                zapVfx.Play();
                break;
            case WeaponElement.ICE:
                icePoofVfx.Play();
                break;
        }
    }
}

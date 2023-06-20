using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactVFX : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] ParticleSystem magicImpact;

    public void AttackImpact()
    {
        if (playerData.equippedEmblems.Contains(emblemLibrary._spellsword) && playerData.mana > emblemLibrary.spellswordManaCost)
        {
            magicImpact.Play();
        }
    }
}

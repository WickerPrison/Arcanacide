using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementalistAnimationEvents : MeleeEnemyAnimationEvents
{
    ElementalistController controller;
    [SerializeField] ParticleSystem swooshShock;

    public override void Start()
    {
        base.Start();
        controller = GetComponentInParent<ElementalistController>();
    }

    public void SwingSword(int smearSpeed)
    {
        controller.SwingSword(smearSpeed);
    }

    public void SwooshShock()
    {
        swooshShock.Play(true);
        controller.canHitPlayer = attackArc.CanHitPlayer();
        controller.SwooshShock();
    }

    public void IceStomp()
    {
        controller.IceStomp();
    }

    public void ChaosHead()
    {
        controller.StartChaosHead();
    }

    public void PlantAttack()
    {
        controller.Bubbles();
    }
}

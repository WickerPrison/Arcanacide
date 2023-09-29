using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantAnimationEvents : MonoBehaviour
{
    AssistantController controller;

    private void Start()
    {
        controller = GetComponentInParent<AssistantController>();
    }

    public void EndAttack(float attackTime)
    {
        controller.EndAttack(attackTime);
    }

    public void ThrowBomb(int hand)
    {
        controller.ThrowBomb(hand);
    }

    public void Beams()
    {
        controller.SummonBeams();
    }

    public void StartBolts()
    {
        controller.StartBolts();
    }

    public void EndBolts()
    {
        controller.EndBolts();
    }

    public void ThrowAC()
    {
        controller.ThrowAC();
    }
}

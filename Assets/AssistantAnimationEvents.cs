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

    public void EndAttack()
    {
        controller.EndAttack();
    }

    public void ThrowBomb(int hand)
    {
        controller.ThrowBomb(hand);
    }
}

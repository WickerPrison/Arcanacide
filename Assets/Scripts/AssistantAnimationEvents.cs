using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistantAnimationEvents : MonoBehaviour
{
    AssistantController controller;
    CameraFollow cameraScript;

    private void Start()
    {
        controller = GetComponentInParent<AssistantController>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
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

    public void IceRings()
    {
        controller.IceRings();
    }

    public void ThrowAC()
    {
        controller.ThrowAC();
    }

    public void ScreenShake()
    {
        StartCoroutine(cameraScript.ScreenShake(0.1f, 0.5f));
    }

    public void BecomeActive()
    {
        controller.BecomeActive();
    }
}

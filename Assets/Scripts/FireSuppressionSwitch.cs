using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireSuppressionSwitch : MonoBehaviour, IFlipOn
{
    [SerializeField] MapData mapData;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem wayFaerie;
    [SerializeField] GameObject message;
    Transform player;
    InputManager im;
    float playerDistance = 100;
    float interactDistance = 2;
    public event EventHandler onFixed;

    private void Start()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InputManager>();
        im.controls.Gameplay.Interact.performed += ctx => FlipSwitch();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        switch (mapData.fireSuppressionState)
        {
            case FireSuppressionState.FIXED:
                wayFaerie.Stop();
                wayFaerie.Clear();
                goto case FireSuppressionState.ON;
            case FireSuppressionState.ON:
                animator.Play("SwitchUp");
                break;
            case FireSuppressionState.OFF:
                animator.Play("SwitchDown");
                break;
        }
        if(mapData.fireSuppressionState == FireSuppressionState.FIXED)
        {
            wayFaerie.Stop();
            wayFaerie.Clear();
        }
        
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (playerDistance <= interactDistance && mapData.fireSuppressionState != FireSuppressionState.FIXED)
        {
            message.SetActive(true);
        }
        else
        {
            message.SetActive(false);
        }
    }

    void FlipSwitch()
    {
        if (playerDistance > interactDistance) return;
        switch (mapData.fireSuppressionState)
        {
            case FireSuppressionState.ON:
                animator.Play("Handle|Pull_Down");
                mapData.fireSuppressionState = FireSuppressionState.OFF;
                break;
            case FireSuppressionState.OFF:
                animator.Play("Handle|Pull_Up");
                mapData.fireSuppressionState = FireSuppressionState.FIXED;
                break;
        }
    }

    public void FlipOn()
    {
        if(mapData.fireSuppressionState == FireSuppressionState.FIXED)
        {
            wayFaerie.Stop();
            wayFaerie.Clear();
            onFixed?.Invoke(this, EventArgs.Empty);
        }
    }
}

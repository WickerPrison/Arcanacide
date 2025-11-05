 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawVFX : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform attackPoint;
    [SerializeField] ParticleSystem[] singleClaw;
    PlayerAnimation playerAnimation;

    private void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
    }

    public void StartVFX(List<Vector3> zRot)
    {
        transform.rotation = Quaternion.LookRotation(attackPoint.position - player.position, Vector3.up);
        transform.localRotation = Quaternion.Euler(
            transform.localRotation.x + zRot[playerAnimation.facingDirection].x, 
            transform.localRotation.y + zRot[playerAnimation.facingDirection].y, 
            zRot[playerAnimation.facingDirection].z
            );
        foreach (ParticleSystem swipe in singleClaw)
        {
            ParticleSystem.MainModule main = swipe.main;
            //main.startRotationY = (attackPoint.rotation.eulerAngles.y + zRot[playerAnimation.facingDirection].y) * Mathf.Deg2Rad;
            //main.startRotationX = zRot[playerAnimation.facingDirection].x * Mathf.Deg2Rad;

            swipe.Play();
        }
    }
}

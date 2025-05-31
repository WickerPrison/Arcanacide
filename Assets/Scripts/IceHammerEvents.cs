using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHammerEvents : MonoBehaviour
{
    IceHammerController hammerController;
    [SerializeField] SpriteRenderer indicatorCircle;
    [SerializeField] SpriteRenderer jumpIndicator;

    private void Start()
    {
        hammerController = GetComponentInParent<IceHammerController>();
    }

    public void ShowAttackCircle(int show)
    {
        indicatorCircle.enabled = show == 1;
    }

    public void AreaSmash()
    {
        indicatorCircle.enabled = false;
        jumpIndicator.enabled = false;
        hammerController.AreaSmash();
    }

    public void StartJump()
    {
        hammerController.StartJump();
    }

    public void EndJump()
    {
        hammerController.jumps -= 1;
        if(hammerController.jumps > 0)
        {
            hammerController.frontAnimator.Play("JumpAgain");
            hammerController.backAnimator.Play("JumpAgain");
        }
        else
        {
            hammerController.frontAnimator.Play("JumpEnd");
            hammerController.backAnimator.Play("JumpEnd");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHammerEvents : MonoBehaviour
{
    IceHammerController hammerController;
    [SerializeField] SpriteRenderer indicatorCircle;
    [SerializeField] SpriteRenderer jumpIndicator;
    EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = GetComponentInParent<EnemyEvents>();
    }

    private void Start()
    {
        hammerController = GetComponentInParent<IceHammerController>();
    }

    public void ShowAttackCircle(int show)
    {
        indicatorCircle.enabled = show == 1;
    }

    public void HammerSmash()
    {
        indicatorCircle.enabled = false;
        jumpIndicator.enabled = false;
        hammerController.HammerSmashImpact();
    }

    public void JumpSmash()
    {
        indicatorCircle.enabled = false;
        jumpIndicator.enabled = false;
        hammerController.JumpSmashImpact();
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
            hammerController.enemyCollider.enabled = true;
        }
    }

    public void Stomp()
    {
        hammerController.Stomp();
    }

    public void ButtSlam()
    {
        hammerController.ButtSlam();
    }

    public void HammerIcicles()
    {
        hammerController.HammerIcicles();
    }

    private void OnEnable()
    {
        enemyEvents.OnStagger += EnemyEvents_OnStagger;
        enemyEvents.OnStartDying += EnemyEvents_OnStartDying;
    }

    private void OnDisable()
    {
        enemyEvents.OnStagger -= EnemyEvents_OnStagger; 
        enemyEvents.OnStartDying += EnemyEvents_OnStartDying;
    }

    private void EnemyEvents_OnStartDying(object sender, System.EventArgs e)
    {
        jumpIndicator.enabled = false;
        indicatorCircle.enabled = false;
    }

    private void EnemyEvents_OnStagger(object sender, System.EventArgs e)
    {
        jumpIndicator.enabled = false;
        indicatorCircle.enabled = false;
    }
}

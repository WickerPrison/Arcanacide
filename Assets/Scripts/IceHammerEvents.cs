using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceHammerEvents : MonoBehaviour
{
    IceHammerController hammerController;
    [SerializeField] SpriteRenderer indicatorCircle;

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
        hammerController.AreaSmash();
    }

    public void StartJump()
    {
        hammerController.StartJump();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGetSummoned
{
    void SetDirection(Vector3 direction);
    void CallAnimation(string animationName);
    void Attack();
    void ShowIndicator();
    void HideIndicator();
    void GoAway();
}

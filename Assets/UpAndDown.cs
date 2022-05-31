using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    float floatTimer;
    float floatMaxTime = 5;
    float floatSpeed = 0.3f;
    int goingUp = 1;

    private void FixedUpdate()
    {
        UpAndDownFunction();
    }

    void UpAndDownFunction()
    {
        if (floatTimer > 0)
        {
            floatTimer -= Time.deltaTime;
            transform.Translate(transform.up * goingUp * Time.fixedDeltaTime * floatSpeed);
        }
        else
        {
            floatTimer = floatMaxTime;
            goingUp *= -1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerSlowAnimationEvents : MonoBehaviour
{
    [SerializeField] FacePlayerSlow facePlayerSlow;

    public void SetRotationSpeed(int speed)
    {
        facePlayerSlow.rotateSpeed = speed;
    }
}

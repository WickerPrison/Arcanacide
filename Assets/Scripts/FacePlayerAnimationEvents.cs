using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayerAnimationEvents : MonoBehaviour
{
    FacePlayer facePlayer;

    private void Start()
    {
        facePlayer = GetComponentInParent<FacePlayer>();
    }

    public void FacePlayer()
    {
        facePlayer.ManualFace();
    }
}

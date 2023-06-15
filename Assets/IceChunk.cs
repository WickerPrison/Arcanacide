using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceChunk : MonoBehaviour
{
    [SerializeField] int limb;
    HalfGolemController controller;

    private void Awake()
    {
        controller = GetComponentInParent<HalfGolemController>();
    }
    private void onIceBreak(object sender, System.EventArgs e)
    {
        if(controller.remainingIce == limb)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        controller.onIceBreak += onIceBreak;
    }

    private void OnDisable()
    {
        controller.onIceBreak -= onIceBreak;
    }
}

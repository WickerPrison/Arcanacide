using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhoneTriggerCollision : PhoneTrigger
{
    public override void Start()
    {
        //override the parent with nothing
    }

    private void OnTriggerEnter(Collider other)
    {
        SetTexts();
    }
}

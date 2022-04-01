using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireBossDoor : Doorway
{
    [SerializeField] GameObject secondMessage;

    // Update is called once per frame
    public override void Update()
    {
        playerDistance = Vector3.Distance(transform.position, player.position);

        if (mapData.ticketFiled)
        {
            base.Update();
        }
        else
        {
            doorOpen = false;
        }

        if (playerDistance <= 2 && !mapData.ticketFiled)
        {
            secondMessage.SetActive(true);
        }
        else
        {
            secondMessage.SetActive(false);
        }
    }
}

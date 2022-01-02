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
        if (mapData.ticketFiled)
        {
            base.Update();
        }

        float playerDistance = Vector3.Distance(transform.position, player.position);
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

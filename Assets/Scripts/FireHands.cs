using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHands : MonoBehaviour
{
    Transform hand;

    // Start is called before the first frame update
    void Start()
    {
        hand = transform.parent.transform;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(hand != null)
        {
            transform.position = hand.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

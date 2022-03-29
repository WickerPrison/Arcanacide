using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHands : MonoBehaviour
{
    Transform hand;
    [SerializeField] bool hasSound = false;
    AudioSource fireSound;

    // Start is called before the first frame update
    void Start()
    {
        hand = transform.parent.transform;
        transform.parent = null;

        if (hasSound)
        {
            fireSound = GetComponent<AudioSource>();
            fireSound.time += Random.Range(0, 0.5f);
        }
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

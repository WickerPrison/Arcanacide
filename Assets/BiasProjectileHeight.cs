using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiasProjectileHeight : MonoBehaviour
{
    [SerializeField] float desiredHeight;
    public float speed;
    float heightDelta;


    private void FixedUpdate()
    {
        heightDelta = desiredHeight - transform.position.y;
        if(Mathf.Abs(heightDelta) > 0.01f)
        {
            transform.position += new Vector3(0, heightDelta * speed * Time.fixedDeltaTime, 0);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, desiredHeight, transform.position.z );
        }
    }
}

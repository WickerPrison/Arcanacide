using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBox : MonoBehaviour
{
    public float rippleSpeed;
    public float lifeTime;
    public Vector3 origin;
    Vector3 direction;

    private void Start()
    {
        direction = transform.position - origin;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * rippleSpeed * Time.fixedDeltaTime);
    }
}

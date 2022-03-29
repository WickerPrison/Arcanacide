using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour
{
    [SerializeField] float timeToDie;

    // Update is called once per frame
    void Update()
    {
        timeToDie -= Time.deltaTime;
        if(timeToDie <= 0)
        {
            Destroy(gameObject);
        }
    }
}

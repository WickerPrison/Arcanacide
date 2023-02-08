using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSpawnerCleanup : MonoBehaviour
{
    [SerializeField] bool activate = false;

    private void OnDrawGizmosSelected()
    {
        if (activate)
        {
            activate = false;
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

            foreach(Rigidbody rigidbody in rigidbodies)
            {
                if(rigidbody.transform.localPosition.magnitude > 100)
                {
                    DestroyImmediate(rigidbody.gameObject);
                }
                else
                {
                    DestroyImmediate(rigidbody);
                }
            }
        }
    }
}

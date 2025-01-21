using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodstains : MonoBehaviour
{
    [SerializeField] GameObject bloodstainPrefab;
    ParticleSystem system;
    List<ParticleCollisionEvent> collisionEvents;

    private void Start()
    {
        system = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }


    private void OnParticleCollision(GameObject other)
    {
        system.GetCollisionEvents(other, collisionEvents);

        for(int i = 0; i < collisionEvents.Count; i++)
        {
            GameObject bloodstain = Instantiate(bloodstainPrefab);
            bloodstain.transform.position = new Vector3(collisionEvents[i].intersection.x, 0, collisionEvents[i].intersection.z);
        }
    }
}

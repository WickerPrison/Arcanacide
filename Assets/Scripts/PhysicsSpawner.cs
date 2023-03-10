using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSpawner : MonoBehaviour
{
    [SerializeField] bool On;
    [SerializeField] List<GameObject> spawnPool;
    [SerializeField] float spawnTimer = 120;
    [SerializeField] float maxSpawnTimer = 120;

    private void OnDrawGizmosSelected()
    {
        Physics.autoSimulation = false;
        Physics.Simulate(Time.fixedDeltaTime);
        Physics.autoSimulation = true;

        if(On && spawnTimer > 0)
        {
            spawnTimer--;
            if(spawnTimer <= 0)
            {
                spawnTimer = maxSpawnTimer;
                SpawnObject();
            }
        }
    }

    void SpawnObject()
    {
        int objectId = Random.Range(0, spawnPool.Count);
        GameObject theObject = Instantiate(spawnPool[objectId]);
        theObject.transform.parent = transform;
        theObject.transform.position = transform.position;
        theObject.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(1, 360), Random.Range(1, 360), Random.Range(1, 360)));
        ObjectHeightSetup objectHeight = theObject.GetComponent<ObjectHeightSetup>();
        if(objectHeight != null)
        {
            objectHeight.lockHeight = false;
        }
        Rigidbody objectRB = theObject.AddComponent<Rigidbody>();
        objectRB.useGravity = true;
        objectRB.velocity = Vector3.zero;
    }
}

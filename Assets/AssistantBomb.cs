using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AssistantBomb : ArcProjectile
{
    float timer;
    int bombCount = 5;
    float miniBombRadius = 5f;
    [SerializeField] GameObject miniBombPrefab;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        timer = timeToHit / 2;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            spriteRenderer.sortingOrder = 0;
        }
    }

    public override void Explosion()
    {
        for(int i = 0; i < bombCount; i++)
        {
            ArcProjectile miniBomb = Instantiate(miniBombPrefab).GetComponent<ArcProjectile>();
            miniBomb.transform.position = transform.position;
            float xPos = Random.Range(-miniBombRadius, miniBombRadius);
            float zPos = Random.Range(-miniBombRadius, miniBombRadius);
            Vector3 endPosition = new Vector3(xPos, 0, zPos) + transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(endPosition, out hit, 6, NavMesh.AllAreas);
            miniBomb.endPoint = hit.position;
            miniBomb.timeToHit += Random.Range(-0.3f, 0.3f);
        }

        base.Explosion();
    }
}
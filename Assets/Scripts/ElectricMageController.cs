using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class ElectricMageController : EnemyController
{
    public GameObject boltsPrefab;
    [SerializeField] Transform frontLightningOrigin;
    [SerializeField] Transform backLightningOrigin;
    [System.NonSerialized] public List<ChainLightningLink> chainLightningLinks = new List<ChainLightningLink>();
    [System.NonSerialized] public List<ChainLightningLink> notElectrifiedLinks = new List<ChainLightningLink>();
    List<ChainLightningLink> nearbyLinks = new List<ChainLightningLink>();
    [SerializeField] float meshRadius;
    List<Bolts> chainLightning = new List<Bolts>();
    Vector3 away = new Vector3(100, 100, 100);
    [System.NonSerialized] public LayerMask layerMask;
    float boltCD = 0;
    [System.NonSerialized] public float boltMaxCD = 0.1f;
    [System.NonSerialized] public int boltDamage = 2;
    [System.NonSerialized] public float boltPoiseDamage = 4;
    bool hasSurrendered = false;
    bool isDying = false;
    ChainLightningLink closestLink;
    bool moving = false;
    bool getNewPoint = true;
    int chainsActive;
    bool soundPlaying = false;

    public override void Start()
    {
        base.Start();
        for(int i = 0; i < 3; i++)
        {
            Bolts bolts = Instantiate(boltsPrefab).GetComponent<Bolts>();
            chainLightning.Add(bolts);
        }
        BoltAway();

        layerMask = LayerMask.GetMask("Player", "IFrames");
    }

    public override void EnemyAI()
    {
        if (hasSurrendered)
        {
            detectionTrigger = true;
        }
        base.EnemyAI();

        if (hasSeenPlayer && !hasSurrendered)
        {
            frontAnimator.SetBool("hasSeenPlayer", true);
            backAnimator.SetBool("hasSeenPlayer", true);
            MovementAI();
        }
    }

    private void FixedUpdate()
    {
        if (isDying)
        {
            return;
        }

        ChainLightning();
        BoltDamage();
    }

    void ChainLightning()
    {
        notElectrifiedLinks.Clear();
        notElectrifiedLinks = new List<ChainLightningLink>(chainLightningLinks);
        nearbyLinks.Clear();
        nearbyLinks = new List<ChainLightningLink>(chainLightningLinks);
        chainsActive = 0;
        for(int i = 0; i < 3; i++)
        {
            float distance = 10;
            foreach(ChainLightningLink link in nearbyLinks)
            {
                float linkDistance = Vector3.Distance(link.transform.position, transform.position);
                if (linkDistance < distance)
                {
                    distance = linkDistance;
                    closestLink = link;
                }
            }

            if(distance < 10)
            {
                chainsActive += 1;
                nearbyLinks.Remove(closestLink);
                if (notElectrifiedLinks.Contains(closestLink))
                {
                    notElectrifiedLinks.Remove(closestLink);
                    closestLink.ChainLightning();
                }

                if (facingFront)
                {
                    chainLightning[i].SetPositions(frontLightningOrigin.position, closestLink.transform.position);
                }
                else
                {
                    chainLightning[i].SetPositions(backLightningOrigin.position, closestLink.transform.position);
                }
            }
            else
            {
                chainLightning[i].SetPositions(away, away);
            }
        }

        if(chainsActive > 0)
        {
            if (!soundPlaying)
            {
                soundPlaying = true;
                enemySound.SFX.Play();
            }
        }
        else
        {
            soundPlaying = false;
            enemySound.SFX.Stop();
        }
    }


    void BoltDamage()
    {
        if(boltCD > 0)
        {
            boltCD -= Time.fixedDeltaTime;
            return;
        }

        bool playerHit = false;
        for(int i = 0; i < 3; i++)
        {
            Vector3 direction = chainLightning[i].endPosition - chainLightning[i].startPosition;
            float distance = Vector3.Distance(chainLightning[i].startPosition, chainLightning[i].endPosition);
            if (Physics.Raycast(chainLightning[i].startPosition, direction, distance, layerMask))
            {
                playerHit = true;
            }
        }

        if (playerHit)
        {
            chainLightning[0].SoundOn();
            playerScript.LoseHealth(boltDamage);
            playerScript.LosePoise(boltPoiseDamage);
            boltCD = boltMaxCD;
        }
        else
        {
            chainLightning[0].SoundOff();
        }
    }


    void Surrender()
    {
        hasSurrendered = true;
        detectionTrigger = false;
        gm.awareEnemies -= 1;
        frontAnimator.Play("Surrender");
        backAnimator.Play("Surrender");
    }

    void BoltAway()
    {
        for(int i = 0; i < 3; i++)
        {
            chainLightning[i].SetPositions(away, away);   
        }
    }

    void MovementAI()
    {
        if (!moving && getNewPoint)
        {
            moving = true;
            getNewPoint = false;
            Vector3 direction = playerScript.transform.position - transform.position;
            Vector3 position = transform.position + direction.normalized * 14;
            position += Random.insideUnitSphere * meshRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit, meshRadius, 1);
            navAgent.SetDestination(hit.position);
        }
        else if(moving && navAgent.remainingDistance < 0.1)
        {
            moving = false;
            StartCoroutine(WaitToMove());
        }
    }

    IEnumerator WaitToMove()
    {
        yield return new WaitForSeconds(2);
        getNewPoint = true;
    }

    public override void StartStagger(float staggerDuration)
    {
        if (!hasSurrendered)
        {
            base.StartStagger(staggerDuration);
        }
    }

    public override void StartDying()
    {
        navAgent.enabled = false;
        isDying = true;
        boltCD = 10000;
        notElectrifiedLinks.Clear();
        notElectrifiedLinks = new List<ChainLightningLink>(chainLightningLinks);
        for(int i = 0; i < 3; i++)
        {
            Destroy(chainLightning[i].gameObject);
        }
        if (hasSurrendered)
        {
            frontAnimator.Play("SurrenderDeath");
            backAnimator.Play("SurrenderDeath");
        }
        else
        {
            frontAnimator.Play("StandingDeath");
            backAnimator.Play("StandingDeath");
        }
    }
}

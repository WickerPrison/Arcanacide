using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningLink : MonoBehaviour
{
    GameManager gm;
    ElectricMageController electricMage;
    ElectricAlly electricAlly;
    GameObject boltsPrefab;
    Bolts bolts;
    PlayerScript playerScript;
    Vector3 away = new Vector3(100, 100, 100);
    ChainLightningLink closestLink;
    float arcLimit = 10;
    bool isElectrified = false;
    float boltCD = 0;

    // Start is called before the first frame update
    void Start()
    {
        electricAlly = GetComponentInParent<ElectricAlly>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        foreach(EnemyScript enemy in gm.enemies)
        {
            ElectricMageController mage = enemy.gameObject.GetComponent<ElectricMageController>();
            if(mage != null)
            {
                mage.chainLightningLinks.Add(this);
                electricMage = mage;
                boltsPrefab = electricMage.boltsPrefab;
            }
        }

        bolts = Instantiate(boltsPrefab).GetComponent<Bolts>();
        bolts.SetPositions(away, away);
    }

    private void Update()
    {
        if (isElectrified && electricMage.notElectrifiedLinks.Contains(this))
        {
            isElectrified = false;
            bolts.SetPositions(away, away);
            if(electricAlly != null)
            {
                electricAlly.ShieldOnOff(isElectrified);
            }
        }
    }

    private void FixedUpdate()
    {
        BoltDamage();
    }

    public void ChainLightning()
    {
        isElectrified = true;

        if(electricAlly != null)
        {
            electricAlly.ShieldOnOff(isElectrified);
        }

        closestLink = null;
        float distance = arcLimit;
        foreach (ChainLightningLink link in electricMage.notElectrifiedLinks)
        {
            float linkDistance = Vector3.Distance(link.transform.position, transform.position);
            if (linkDistance < distance)
            {
                distance = linkDistance;
                closestLink = link;
            }
        }

        if(closestLink != null)
        {
            bolts.SetPositions(transform.position, closestLink.transform.position);

            electricMage.notElectrifiedLinks.Remove(closestLink);
            closestLink.ChainLightning();
        }
        else
        {
            bolts.SetPositions(away, away);
        }
    }

    void BoltDamage()
    {
        if (boltCD > 0)
        {
            boltCD -= Time.fixedDeltaTime;
            return;
        }

        Vector3 direction = bolts.endPosition - bolts.startPosition;
        float distance = Vector3.Distance(bolts.startPosition, bolts.endPosition);
        if (Physics.Raycast(bolts.startPosition, direction, distance, electricMage.layerMask))
        {
            playerScript.LoseHealth(electricMage.boltDamage);
            playerScript.LosePoise(electricMage.boltPoiseDamage);
            boltCD = electricMage.boltMaxCD;
        }
    }

    private void OnDestroy()
    {
        if(electricMage != null)
        {
            electricMage.chainLightningLinks.Remove(this);
        }
    }
}

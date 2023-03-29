using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleMine : MonoBehaviour
{
    ElectricBossController bossController;
    ElectricPuddleScript puddleScript;
    float triggerDistance = 5;
    float triggerCooldown = 2;
    bool canTrigger = true;
    [SerializeField] GameObject boltsPrefab;
    Bolts bolts;
    Vector3 away = new Vector3(100, 100, 100);
    bool bolting = false;
    float boltTimer;
    float boltMaxTime = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        bossController = GameObject.FindGameObjectWithTag("Enemy").GetComponent<ElectricBossController>(); 
        puddleScript = GetComponent<ElectricPuddleScript>();
        bolts = Instantiate(boltsPrefab).GetComponent<Bolts>();
        bolts.SetPositions(away, away);
    }

    private void Update()
    {
        if(bossController == null)
        {
            bolts.SetPositions(away, away);
            puddleScript.PowerOff();
            return;
        }

        if (bolting)
        {
            bolts.SetPositions(transform.position, bossController.transform.position);
            boltTimer -= Time.deltaTime;
            if(boltTimer <= 0)
            {
                bolts.SetPositions(away, away);
                bolting = false;
            }
        }


        if (!bossController.phase2 || !canTrigger)
        {
            return;
        }

        float bossDistance = Vector3.Distance(transform.position, bossController.transform.position); 
        if(bossDistance < triggerDistance)
        {
            puddleScript.PowerOn();
            boltTimer = boltMaxTime;
            bolting = true;
            canTrigger = false;
            StartCoroutine(TriggerCooldown());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            puddleScript.PowerOff();
        }
    }

    IEnumerator TriggerCooldown()
    {
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}

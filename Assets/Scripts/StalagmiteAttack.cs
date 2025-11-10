using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.AI;

public class StalagmiteAttack : MonoBehaviour
{
    [SerializeField] SpriteRenderer tallIcicle;
    [SerializeField] Vector3 tallIcicleStart;
    [SerializeField] Vector3 tallIcicleFinal;
    [SerializeField] SpriteRenderer bigIcicle;
    [SerializeField] Vector3 bigIcicleStart;
    [SerializeField] Vector3 bigIcicleFinal;
    [SerializeField] float emergeTime;
    [SerializeField] float peakTime;
    WaitForSeconds peakWait;
    [SerializeField] float retractTime;
    [SerializeField] int damage;
    [SerializeField] float poiseDamage;
    Collider hitbox;
    SpriteRenderer icicle;
    Vector3 start;
    Vector3 end;
    PlayerScript playerScript;
    [System.NonSerialized] public EnemyScript enemyOfOrigin;
    [SerializeField] EventReference iceImpact;
    Vector3 localPos;
    Transform holder;
    [SerializeField] bool attackEnemy;
    [SerializeField] bool showInEditor;
    [System.NonSerialized] public bool isTriggered = false;

    private void Awake()
    {
        int randInt = Random.Range(0, 2);
        ResetIcicles();
        if (randInt == 0)
        {
            icicle = tallIcicle;
            start = tallIcicleStart;
            end = tallIcicleFinal;
        }
        else
        {
            icicle = bigIcicle;
            start = bigIcicleStart;
            end = bigIcicleFinal;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        peakWait = new WaitForSeconds(peakTime);
        hitbox = GetComponent<Collider>();
        hitbox.enabled = false;
        localPos = transform.localPosition;
        holder = transform.parent;
    }

    void ResetIcicles()
    {
        tallIcicle.transform.localPosition = tallIcicleStart;
        bigIcicle.transform.localPosition = bigIcicleStart;
        tallIcicle.enabled = false;
        bigIcicle.enabled = false;
    }

    public void Trigger()
    {
        NavMeshHit navMeshHit;
        if(NavMesh.SamplePosition(transform.position, out navMeshHit, 0.1f, NavMesh.AllAreas))
        {
            StartCoroutine(Emerge(icicle, start, end));
        }
    }

    public void TriggerNoNavmesh()
    {
        StartCoroutine(Emerge(icicle, start, end));
    }

    public IEnumerator Emerge(SpriteRenderer icicle, Vector3 start, Vector3 end)
    {
        icicle.transform.rotation = Quaternion.Euler(25, 0, 180);
        icicle.enabled = true;
        isTriggered = true;
        float emergeTimer = 0;
        transform.SetParent(null);
        while(emergeTimer < emergeTime)
        {
            icicle.transform.localPosition = Vector3.Lerp(start, end, emergeTimer / emergeTime);
            emergeTimer += Time.deltaTime;
            yield return null;
        }

        hitbox.enabled = true;
        yield return peakWait;
        hitbox.enabled = false;

        emergeTimer = 0;
        while (emergeTimer < retractTime)
        {
            icicle.transform.localPosition = Vector3.Lerp(end, start, emergeTimer / retractTime);
            emergeTimer += Time.deltaTime;
            yield return null;
        }
        icicle.enabled = false;
        transform.SetParent(holder);
        transform.localPosition = localPos;
        isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackEnemy)
        {
            HitPlayer(other);
        }
    }

    void HitPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerScript == null)
            {
                playerScript = other.GetComponent<PlayerScript>();
            }

            playerScript.HitPlayer(() =>
            {
                playerScript.LoseHealth(damage, EnemyAttackType.NONPARRIABLE, enemyOfOrigin);
                playerScript.LosePoise(poiseDamage);
                playerScript.StartStagger(0.2f);
                RuntimeManager.PlayOneShot(iceImpact, 1f);
                hitbox.enabled = false;
            }, () =>
            {
                playerScript.PerfectDodge(EnemyAttackType.NONPARRIABLE, enemyOfOrigin);
            });
        }
    }

    public void CancelStalagmite()
    {
        StopAllCoroutines();
        ResetIcicles();
    }

    public void ColorChange(Color oldColor, Color newColor)
    {
        icicle.material.SetFloat("_ColorChange", 1);
        icicle.material.SetColor("_OriginalColor", oldColor);
        icicle.material.SetColor("_NewColor", newColor);
    }

    private void OnValidate()
    {
        if(tallIcicle.enabled != showInEditor)
        {
            tallIcicle.enabled = showInEditor;
            bigIcicle.enabled = showInEditor;
        }
    }
}

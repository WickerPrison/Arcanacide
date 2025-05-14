using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HarpoonManager : MonoBehaviour
{
    [SerializeField] GameObject boltsPrefab;
    [SerializeField] int boltDamage;
    [SerializeField] float boltPoiseDamage;
    [SerializeField] float boltCD;
    [SerializeField] Vector2 harpoonBoundaries;
    float boltTimer;
    List<TeslaHarpoon> harpoons = new List<TeslaHarpoon>();
    [System.NonSerialized] public List<TeslaHarpoonProjectile> harpoonProjectiles = new List<TeslaHarpoonProjectile>();
    List<Bolts> bolts = new List<Bolts>();
    List<(Vector3, Vector3)> pairs = new List<(Vector3, Vector3)>();
    Vector3 away = new Vector3(1000, 1000, 1000);
    LayerMask layerMask;
    PlayerScript playerScript;
    public float spacing;
    Vector3 vertPositioning = new Vector3(0, 15, 0);

    private void Start()
    {
        layerMask = LayerMask.GetMask("Player");
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Update()
    {
        if (pairs.Count == 0) return;
        if(boltTimer > 0)
        {
            boltTimer -= Time.deltaTime;
            return;
        }

        bool hitPlayer = false;
        foreach((Vector3, Vector3) pair in pairs)
        {
            if(Physics.Linecast(pair.Item1, pair.Item2, layerMask))
            {
                hitPlayer = true;
                break;
            }
        }

        if (hitPlayer)
        {
            boltTimer = boltCD;
            bolts[0].SoundOn();
            playerScript.LoseHealth(boltDamage, EnemyAttackType.NONPARRIABLE, null);
            playerScript.LosePoise(boltPoiseDamage);
        }
        else
        {
            bolts[0].SoundOff();
        }
    }

    public void AddHarpoon(TeslaHarpoon harpoon)
    {
        harpoons.Add(harpoon);
        UpdateBolts();
    }

    public void RemoveHarpoon(TeslaHarpoon harpoon)
    {
        harpoons.Remove(harpoon);
        UpdateBolts();
    }

    void UpdateBolts()
    {
        pairs.Clear();
        for (int i = 0; i < harpoons.Count - 1; i++)
        {
            for (int j = i + 1; j < harpoons.Count; j++)
            {
                pairs.Add((harpoons[i].lightningOrigin.position, harpoons[j].lightningOrigin.position));
            }
        }

        while (bolts.Count < pairs.Count)
        {
            bolts.Add(Instantiate(boltsPrefab).GetComponent<Bolts>());
        }

        for (int i = 0; i < bolts.Count; i++)
        {
            if (i < pairs.Count)
            {
                bolts[i].SetPositions(pairs[i]);
            }
            else
            {
                bolts[i].SetPositions(away, away);
            }
        }
    }

    public Vector3 GetHarpoonPosition()
    {
        Vector3 attempt = Vector3.zero;
        for(int i = 0; i < 10; i++)
        {
            attempt = new Vector3(
                Random.Range(-harpoonBoundaries.x, harpoonBoundaries.x), 
                0, 
                Random.Range(-harpoonBoundaries.y, harpoonBoundaries.y
            ));
            attempt = Utils.RotateDirection(attempt, -45);
            bool enoughSpace = true;
            foreach(TeslaHarpoon harpoon in harpoons)
            {
                if(Vector3.Distance(attempt, harpoon.transform.position) < spacing)
                {
                    enoughSpace = false;
                    break;
                }
            }
            if (enoughSpace)
            {
                foreach (TeslaHarpoonProjectile projectile in harpoonProjectiles)
                {
                    Vector3 projectilePos = new Vector3(projectile.transform.position.x, 0, projectile.transform.position.z);
                    if (Vector3.Distance(attempt, projectilePos) < spacing)
                    {
                        enoughSpace = false;
                        break;
                    }
                }
            }
            if (enoughSpace) return attempt;
        }
        Debug.Log("too many attempts");
        return attempt;
    }

    public int GetCount()
    {
        return pairs.Count;
    }

    public List<float> GetDistances()
    {
        return pairs.Select(pair =>  Vector3.Distance(pair.Item1, pair.Item2)).ToList();
    }

    public void SetupTest(GameObject prefab, int damage, float poiseDamage, float cd)
    {
        boltsPrefab = prefab;
        boltDamage = damage;
        boltPoiseDamage = poiseDamage;
        boltCD = cd;
    }
}



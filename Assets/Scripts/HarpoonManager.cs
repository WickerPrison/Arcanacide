using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonManager : MonoBehaviour
{
    [SerializeField] GameObject boltsPrefab;
    int boltDamage;
    [SerializeField] float boltPoiseDamage;
    float boltCD;
    float boltTimer;
    List<TeslaHarpoon> harpoons = new List<TeslaHarpoon>();
    List<Bolts> bolts = new List<Bolts>();
    List<(Vector3, Vector3)> pairs = new List<(Vector3, Vector3)>();
    Vector3 away = new Vector3(1000, 1000, 1000);
    LayerMask layerMask;
    PlayerScript playerScript;

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

    public int GetCount()
    {
        return pairs.Count;
    }

    public void SetupTest(GameObject prefab, int damage, float cd)
    {
        boltsPrefab = prefab;
        boltDamage = damage;
        boltCD = cd;
    }
}



using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolts : MonoBehaviour
{
    [SerializeField] List<LightningBolt> lightningBolts = new List<LightningBolt>();
    [System.NonSerialized] public Vector3 startPosition;
    [System.NonSerialized] public Vector3 endPosition;
    bool soundOn;
    StudioEventEmitter sfx;
    WaitForSeconds boltFlash = new WaitForSeconds(0.2f);
    Vector3 away = Vector3.one * 100;

    private void Awake()
    {
        sfx = GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            lightningBolts[i].frameCounter = i;
        }
    }

    public void SetPositions(Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        foreach (LightningBolt bolt in lightningBolts)
        {
            bolt.SetPositions(startPosition, endPosition);
        }
    }

    public void SetPositions((Vector3, Vector3) tuple)
    {
        SetPositions(tuple.Item1, tuple.Item2);
    }

    public void SetIndividualBoltPosition(Vector3 startPos, Vector3 endPos, int index)
    {
        lightningBolts[index].SetPositions(startPos, endPos);
    }

    public void BoltsAoeAttackVfx(List<Vector3> targets, Vector3 origin)
    {
        StartCoroutine(BoltsAoeVfx(targets, origin));
    }

    IEnumerator BoltsAoeVfx(List<Vector3> targets, Vector3 origin)
    {
        for(int i = 0; i < targets.Count; i++)
        {
            SetIndividualBoltPosition(origin, targets[i], i);
        }
        yield return boltFlash;
        SetPositions(away, away);
    }

    public void SoundOn()
    {
        if (!soundOn)
        {
            soundOn = true;
            sfx.Play();
        }
    }

    public void SoundOff()
    {
        if(soundOn)
        {
            sfx.Stop();
            soundOn = false;
        }
    }

    public void ToggleBolts(bool turnOn)
    {
        foreach(LightningBolt bolt in lightningBolts)
        {
            bolt.lineRenderer.enabled = turnOn;
        }
    }
}

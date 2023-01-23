using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bolts : MonoBehaviour
{
    [SerializeField] List<LightningBolt> lightningBolts = new List<LightningBolt>();

    private void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            lightningBolts[i].frameCounter = i;
        }
    }

    public void SetPositions(Vector3 startPosition, Vector3 endPosition)
    {
        foreach (LightningBolt bolt in lightningBolts)
        {
            bolt.SetPositions(startPosition, endPosition);
        }
    }
}

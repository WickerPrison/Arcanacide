using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPuddleScript : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] int powerSwitchNumber;
    [SerializeField] List<Collider> colliders;
    [SerializeField] ParticleSystem particles;
    bool switchFlipped = false;

    // Start is called before the first frame update
    void Start()
    {
        if (mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
            FlipSwitch();
        }
    }

    private void Update()
    {
        if (!switchFlipped && mapData.powerSwitchesFlipped.Contains(powerSwitchNumber))
        {
                FlipSwitch();
        }
    }

    void FlipSwitch()
    {
        switchFlipped = true;
        particles.Stop();
        foreach (Collider collider in colliders)
        {
            collider.isTrigger = true;
        }
    }
}

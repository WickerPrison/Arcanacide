using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayFairie : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] bool showWhenLocked = true;
    ParticleSystem particles;

    private void Start()
    {
        bool doorUnlocked = mapData.unlockedDoors.Contains(3);
        if (showWhenLocked && doorUnlocked) return;
        if (!showWhenLocked && !doorUnlocked) return;

        if (playerData.equippedEmblems.Contains("WayFinder"))
        {
            particles = GetComponentInChildren<ParticleSystem>();
            particles.Play();
        }
    }
}

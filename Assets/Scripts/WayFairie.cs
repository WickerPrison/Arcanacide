using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayFairie : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] EmblemLibrary emblemLibrary;
    [SerializeField] bool showWhenLocked = true;
    [SerializeField] bool showWhenUnlocked = true;
    ParticleSystem particles;

    private void Start()
    {
        if (!playerData.equippedEmblems.Contains(emblemLibrary.way_faerie)) return;
            
        bool doorUnlocked = mapData.unlockedDoors.Contains(3);
        if (doorUnlocked && showWhenUnlocked)
        {
            ShowWayfinder();
        }
        else if(!doorUnlocked && showWhenLocked)
        {
            ShowWayfinder();
        }
    }

    void ShowWayfinder()
    {
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Play();
    }
}

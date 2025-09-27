using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayFaerieManager : MonoBehaviour
{
    [SerializeField] WayFairie doorIfLocked;
    [SerializeField] WayFairie doorIfUnlocked;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] MapData mapData;

    // Start is called before the first frame update
    void Start()
    {
        if (!playerData.hasWayfaerie) return;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        WayFaerieProjectile projectile = GameObject.Instantiate(projectilePrefab).GetComponent<WayFaerieProjectile>();
        projectile.start = playerTransform.position + Vector3.up * 1.5f;
        if (mapData.unlockedDoors.Contains(7))
        {
            projectile.destination = doorIfUnlocked;
        }
        else
        {
            projectile.destination = doorIfLocked;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeElectricSmear : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    [SerializeField] PlayerData playerData;
    [SerializeField] List<Vector3> smearScales = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        playerAnimation = GetComponentInParent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerData.currentWeapon == 2)
        {
            transform.localScale = smearScales[playerAnimation.facingDirection];
        }
    }
}

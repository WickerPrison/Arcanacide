using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRefundStone : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] Image gem;
    [SerializeField] Image fill;
    [SerializeField] List<Sprite> gemSprites;


    public void SetGem(int index)
    {
        if (playerData.maxHealCharges == 4) gemSprites.Insert(1, gemSprites[1]);
        fill.enabled = index != playerData.maxHealCharges + 1;
        gem.enabled = index <= playerData.maxHealCharges + 1;
        if(index == playerData.maxHealCharges + 1)
        {
            gem.sprite = gemSprites[gemSprites.Count - 1];
        }
        else if(index < playerData.maxHealCharges + 1)
        {
            gem.sprite = gemSprites[index];
        }
    }
}

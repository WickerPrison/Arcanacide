using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuRefundStone : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] Image background;
    [SerializeField] Image gem;
    [SerializeField] Image fill;
    [SerializeField] List<Sprite> gemSprites;


    private void Awake()
    {
        if (playerData.maxHealCharges == 4) gemSprites.Insert(1, gemSprites[1]);
    }

    private void Start()
    {
        gem.material = new Material(gem.material);
    }

    public void SetGem(int index)
    {
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

    public void EmpowerGem(int index)
    {
        if (playerData.maxHealCharges == 4) gemSprites.Insert(1, gemSprites[1]);
        StartCoroutine(GemFlash(index));
    }

    IEnumerator GemFlash(int index)
    {
        gem.material.SetFloat("_BlackToWhite", 1);
        background.color = Color.white;
        SetGem(index);
        yield return new WaitForSeconds(0.2f);
        gem.material.SetFloat("_BlackToWhite", 0);
        background.color = Color.black;
    }
}

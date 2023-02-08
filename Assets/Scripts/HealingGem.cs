using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingGem : MonoBehaviour
{
    SpriteRenderer gemSprite;
    GameObject fill;
    HUD hud;

    // Start is called before the first frame update
    void Start()
    {
        gemSprite = GetComponent<SpriteRenderer>();
        fill = transform.GetChild(0).gameObject;
        hud = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
        gemSprite.sprite = hud.gemImage.sprite;
        fill.SetActive(gemSprite.sprite != hud.gemSprites[0]);
    }
}

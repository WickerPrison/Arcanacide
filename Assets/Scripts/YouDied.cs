using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class YouDied : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI youDiedText;
    HUD hud;
    public PlayerScript playerScript;
    float messageDuration = 2;

    public void ShowMessage()
    {
        hud = GetComponentInParent<HUD>(); 
        Time.timeScale = 0;
        youDiedText.gameObject.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        yield return hud.FadeToBlack(messageDuration);
        Time.timeScale = 1;
        playerScript.Death();
    }
}

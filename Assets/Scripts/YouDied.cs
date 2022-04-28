using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class YouDied : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI youDiedText;
    [SerializeField] Image fadeToBlack;
    public PlayerScript playerScript;
    float messageDuration = 2;
    bool hasDied = false;
    float endMesageTime;

    public void ShowMessage()
    {
        Time.timeScale = 0;
        youDiedText.gameObject.SetActive(true);
        fadeToBlack.gameObject.SetActive(true);
        hasDied = true;
        endMesageTime = Time.realtimeSinceStartup + messageDuration;
    }

    private void Update()
    {
        if (hasDied)
        {
            if(Time.realtimeSinceStartup >= endMesageTime)
            {
                Time.timeScale = 1;
                playerScript.Death();
            }

            FadeToBlack();
        }
    }

    void FadeToBlack()
    {
        float fadePercent = (messageDuration - (endMesageTime - Time.realtimeSinceStartup)) / messageDuration;
        Color color = fadeToBlack.color;
        color.a = fadePercent;
        fadeToBlack.color = color;
    }
}

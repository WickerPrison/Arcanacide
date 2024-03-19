using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    [SerializeField] Image logo;
    [SerializeField] TextMeshProUGUI brokenBeaker;
    [SerializeField] TextMeshProUGUI coby;
    [SerializeField] TextMeshProUGUI humanDLC;
    [SerializeField] TextMeshProUGUI jacob;
    Color invisible = new Color(1, 1, 1, 0);
    float timer;
    Color fadeColor;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        yield return new WaitForSeconds(2);
        float logoFadeIn = 2;
        timer = logoFadeIn;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            fadeColor = Color.Lerp(Color.white, invisible, timer / logoFadeIn);
            logo.color = fadeColor;
            brokenBeaker.color = fadeColor;
            yield return endOfFrame;
        }

        yield return new WaitForSeconds(2);

        logo.color = invisible;
        brokenBeaker.color = invisible;

        yield return new WaitForSeconds(1);

        float creditsFadeIn = 2;
        timer = creditsFadeIn;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            fadeColor = Color.Lerp(Color.white, invisible, timer / creditsFadeIn);
            coby.color = fadeColor;
            yield return endOfFrame;
        }

        timer = creditsFadeIn;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeColor = Color.Lerp(Color.white, invisible, timer / creditsFadeIn);
            humanDLC.color = fadeColor;
            yield return endOfFrame;
        }

        timer = creditsFadeIn;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fadeColor = Color.Lerp(Color.white, invisible, timer / creditsFadeIn);
            jacob.color = fadeColor;
            yield return endOfFrame;
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
    }
}

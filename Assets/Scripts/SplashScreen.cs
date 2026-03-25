using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI brokenBeakerText;
    [SerializeField] SpriteRenderer brokenBeakerLogo;
    [SerializeField] GameObject controllerRecommended;
    [SerializeField] GameObject fmod;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        fmod.SetActive(false);
        yield return new WaitForSeconds(2);
        brokenBeakerText.enabled = false;
        brokenBeakerLogo.enabled = false;
        fmod.SetActive(true);
        yield return new WaitForSeconds(2);
        controllerRecommended.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainMenu");
    }
}

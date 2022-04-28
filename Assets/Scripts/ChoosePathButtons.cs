using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePathButtons : MonoBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject buttonAudioPrefab;
    SoundManager sm;

    private void Start()
    {
        sm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SoundManager>();
    }

    public void PathOfTheSword()
    {
        ButtonSound();
        playerData.path = "Sword";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfTheDying()
    {
        ButtonSound();
        playerData.path = "Dying";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfThePath()
    {
        ButtonSound();
        playerData.path = "Path";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfThePathless()
    {
        ButtonSound();
        playerData.path = "Pathless";
        SceneManager.LoadScene("Tutorial1");
    }

    void ButtonSound()
    {
        sm.ButtonSound();
    }
}

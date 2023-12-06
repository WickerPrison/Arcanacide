using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    public void NextScreen()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void StartMusicOutro()
    {
        MusicManager musicManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MusicManager>();
        musicManager.ChangeMusicState(MusicState.OUTRO);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    public void NextScreen()
    {
        SceneManager.LoadScene("ChoosePath");
    }
}

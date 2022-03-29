using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePathButtons : MonoBehaviour
{
    [SerializeField] PlayerData playerData;

    public void PathOfTheSword()
    {
        playerData.path = "Sword";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfTheDying()
    {
        playerData.path = "Dying";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfThePath()
    {
        playerData.path = "Path";
        SceneManager.LoadScene("Tutorial1");
    }

    public void PathOfThePathless()
    {
        playerData.path = "Pathless";
        SceneManager.LoadScene("Tutorial1");
    }
}

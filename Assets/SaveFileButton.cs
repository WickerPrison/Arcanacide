using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileButton : MonoBehaviour
{
    [SerializeField] int fileID;
    [SerializeField] MenuData menuData;
    [SerializeField] PlayerData playerData;
    TextMeshProUGUI text;
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Save File " + fileID.ToString();
    }

    public void LoadGame()
    {
        if (menuData.loadGame)
        {
            gm.LoadGame("saveFile" +  fileID.ToString());
        }
        else
        {
            StartNewGame();   
        }
    }

    void StartNewGame()
    {
        gm.NewGame("saveFile" + fileID.ToString());
        SceneManager.LoadScene("IntroCutscene");
    }
}

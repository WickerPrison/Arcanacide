using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveFileButton : MonoBehaviour
{
    [SerializeField] int fileID;
    [SerializeField] MenuData menuData;
    [SerializeField] PlayerData playerData;
    [SerializeField] TextMeshProUGUI fileIDText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] TextMeshProUGUI timeText;
    TextMeshProUGUI text;
    GameManager gm;
    string textString;
    bool haveOldData = false;
    ButtonVibrate buttonVibrate;

    private void Start()
    {
        buttonVibrate = GetComponent<ButtonVibrate>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        textString = "Save File " + fileID.ToString() + " - ";

        SaveData data = null;

        if(fileID == 1)
        {
            data = SaveSystem.LoadGame("playerData");
            if (data != null) haveOldData = true; 
        }

        if(!haveOldData)
        {
            data = SaveSystem.LoadGame("saveFile" + fileID.ToString());
        }

        if(data == null)
        {
            textString += "Empty";
            dateText.text = "";
            timeText.text = "";
        }
        else
        {
            if (haveOldData)
            {
                textString += "Pre version 0.2.11 savefile";
                dateText.text = "";
                timeText.text = "";
            }
            else
            {
                dateText.text = data.date;
                timeText.text = data.time;
            }
        }
        text.text = textString;
    }

    public void LoadGame()
    {
        if (menuData.loadGame)
        {
            if (haveOldData)
            {
                gm.LoadGame("playerData");
                playerData.saveFile = "saveFile1";
                gm.SaveGame();
                File.Delete(Application.persistentDataPath + "/playerData.sav");
            }

            if(File.Exists(Application.persistentDataPath + "/" + "saveFile" + fileID.ToString() + ".sav"))
            {
                gm.LoadGame("saveFile" +  fileID.ToString());
                gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
                string sceneName = gm.GetSceneName(playerData.lastSwordSite);
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                buttonVibrate.StartVibrate();
            }
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

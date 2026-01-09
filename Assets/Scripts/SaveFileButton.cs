using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

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
    ButtonVibrate buttonVibrate;

    private void Start()
    {
        buttonVibrate = GetComponent<ButtonVibrate>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        textString = "Save File " + fileID.ToString() + " - ";

        SaveData data = SaveSystem.LoadGame("saveFile" + fileID.ToString());

        if (data == null)
        {
            textString += "Empty";
            dateText.text = "";
            timeText.text = "";
        }
        else
        {
            if(data.timeOfLastSave == null)
            {
                dateText.text = "Date cannot be displayed due to old save file format.";
                timeText.text = "Gameplay will not be affected.";
            }
            else
            {
                dateText.text = $"Last Played: {DateTime.Parse(data.timeOfLastSave).GetDateTimeFormats('d')[0]} {DateTime.Parse(data.timeOfLastSave).GetDateTimeFormats('t')[0]}";
                TimeSpan playtime = TimeSpan.FromSeconds(data.playtime);
                timeText.text = $"Total Playtime: {(int)playtime.TotalHours}:{playtime.Minutes:00}:{playtime.Seconds:00}";
            }
        }
        text.text = textString;
    }

    public void LoadGame()
    {
        if (menuData.loadGame)
        {

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
        gm.NewGame(fileID);
        SceneManager.LoadScene("IntroCutscene");
    }
}

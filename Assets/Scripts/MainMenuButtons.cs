using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] PlayerData playerData;
    [SerializeField] MapData mapData;
    [SerializeField] MenuData menuData;
    [SerializeField] BuildMode buildMode;
    [SerializeField] GameObject optionsMenuPrefab;
    [SerializeField] GameObject firstButton;
    [SerializeField] GameObject optionsButton;
    SoundManager sm;

    private void Start()
    {
        Time.timeScale = 1;
        sm = gm.gameObject.GetComponent<SoundManager>();
        gm.LoadSettings();
        GlobalEvents.instance.OnChangedSetting();
    }

    public void Load()
    {
        ButtonSound();
        menuData.loadGame = true;
        SceneManager.LoadScene("LoadScreen");
    }

    public void NewGame()
    {
        ButtonSound();
        menuData.loadGame = false;
        SceneManager.LoadScene("LoadScreen");
    }

    public void Options()
    {
        SettingsMenu settingsMenu = Instantiate(optionsMenuPrefab).GetComponent<SettingsMenu>();
        settingsMenu.ActivateBackground();
        settingsMenu.mainMenuButton = optionsButton;
    }

    public void TestFloor2()
    {
        ButtonSound();
        gm.StartAtFloor2();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("ElectricHub");
    }

    public void TestFloor3()
    {
        ButtonSound();
        gm.StartAtFloor3();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("IceHub1");
    }

    public void TestFloor4()
    {
        ButtonSound();
        gm.StartAtFloor4();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        SceneManager.LoadScene("IceElevator2");
    }

    public void TestMinibossV1(int extraMoney)
    {
        ButtonSound();
        gm.NewGame(4);
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        playerData.hasHealthGem = true;
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("FireHub1");
    }

    public void TestFireBoss(int extraMoney)
    {
        ButtonSound();
        gm.NewGame(4);
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        mapData.ticketFiled = true;
        playerData.UnlockAllWeapons();
        playerData.hasHealthGem = true;
        SceneManager.LoadScene("FireHub2");
    }

    public void TestMinibossV2(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor2();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("ElectricHub2");
    }

    public void TestElectricBoss(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor2();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        playerData.UnlockAllWeapons();
        mapData.carolsDeadFriends.Add("Jeff");
        mapData.carolsDeadFriends.Add("Harold");
        mapData.carolsDeadFriends.Add("Arnold");
        SceneManager.LoadScene("ElectricHub");
    }

    public void TestMinibossV3(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor3();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        mapData.hasRemoteAC = true;
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("IceHub1");
    }

    public void TestIceBoss(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor3();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        mapData.hasRemoteAC = true;
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("IceHub1");
    }

    public void TestMinibossV4(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor4();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        mapData.hasRemoteAC = true;
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("ChaosHub3");
    }

    public void TestChaosBoss(int extraMoney)
    {
        ButtonSound();
        gm.StartAtFloor4();
        gm.GetComponent<MusicManager>().ChangeMusicState(MusicState.OUTRO);
        playerData.money += extraMoney;
        mapData.UnlockAllDoors();
        mapData.hasRemoteAC = true;
        playerData.UnlockAllWeapons();
        SceneManager.LoadScene("ChaosHub1");
    }

    public void QuitGame()
    {
        ButtonSound();
        if(buildMode.buildMode == BuildModes.FULLGAME)
        {
            SceneManager.LoadScene("SurveyScreen");
        }
        else
        {
            Application.Quit();
        }
    }

    void ButtonSound()
    {
        sm.ButtonSound();
    }
}

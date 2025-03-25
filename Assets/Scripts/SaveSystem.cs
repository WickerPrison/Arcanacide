using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame (string savePath, PlayerData playerData, MapData mapData, DialogueData phoneData, EmblemLibrary emblemLibrary)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + savePath + ".sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(playerData, mapData, phoneData, emblemLibrary);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame(string loadPath)
    {
        string path = Application.persistentDataPath + "/" + loadPath + ".sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void SaveSettings(SettingsData settingsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.sav";
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsSaveData data = new SettingsSaveData(settingsData);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsSaveData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.sav";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsSaveData data = formatter.Deserialize(stream) as SettingsSaveData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}

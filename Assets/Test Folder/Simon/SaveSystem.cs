using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    private string playerStatsSaveFileDirectory;
    private string dataSaveFileDirectory;

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
        #endregion

        playerStatsSaveFileDirectory = Application.dataPath + "/_Scripts/Save System/pSaveFile.json";
        dataSaveFileDirectory = Application.dataPath + "/_Scripts/Save System/dSaveFile.json";
    }

    /// <summary>
    /// Checks if a game save file has been created and loads it.
    /// If a save file has not been created, it will use the constructor of PlayerStats to load base values.
    /// </summary>
    public PlayerData LoadGameSave()
    {
        Debug.Log("Application " + Application.dataPath + "\nFull " + playerStatsSaveFileDirectory);

        PlayerData loadedData;

        if (File.Exists(playerStatsSaveFileDirectory))
        {
            string savedData = File.ReadAllText(playerStatsSaveFileDirectory);
            loadedData = JsonUtility.FromJson<PlayerData>(savedData);
        }
        else
        {
            loadedData = new PlayerData();
        }

        return loadedData;
    }

    /// <summary>
    /// Checks if a data save file has been created and loads it.
    /// If a save file has not been created, it will use the constructor of Data to load base values.
    /// </summary>
    public Data LoadData()
    {
        Debug.Log("Application " + Application.dataPath + "\nFull " + dataSaveFileDirectory);

        Data loadedData;

        if (File.Exists(dataSaveFileDirectory))
        {
            string savedData = File.ReadAllText(dataSaveFileDirectory);
            loadedData = JsonUtility.FromJson<Data>(savedData);
        }
        else
        {
            loadedData = new Data();
        }

        return loadedData;
    }

    /// <summary>
    /// Saves the given object game data in a json file, file can be found in Save System folder as "pSaveFile.json".
    /// </summary>
    public void SaveGame(PlayerData playerData)
    {
        string data = JsonUtility.ToJson(playerData);

        if (File.Exists(playerStatsSaveFileDirectory))
        {
            File.Delete(playerStatsSaveFileDirectory);
            File.WriteAllText(playerStatsSaveFileDirectory, data);
        }
        else
        {
            File.WriteAllText(playerStatsSaveFileDirectory, data);
        }

        Debug.Log("Game save created.");
    }

    /// <summary>
    /// Saves the given object data in a json file, file can be found in Save System folder as "dSaveFile.json".
    /// </summary>
    /// <param name="gameData">The current data to be saved</param>
    public void SaveData(Data gameData)
    {
        string data = JsonUtility.ToJson(gameData);

        if (File.Exists(dataSaveFileDirectory))
        {
            File.Delete(dataSaveFileDirectory);
            File.WriteAllText(dataSaveFileDirectory, data);
        }
        else
        {
            File.WriteAllText(dataSaveFileDirectory, data);
        }

        Debug.Log("Data save created.");
    }

    /// <summary>
    /// Checks if a game save file exists and deletes it if it does.
    /// </summary>
    public void ClearGameSave()
    {
        if (File.Exists(playerStatsSaveFileDirectory))
            File.Delete(playerStatsSaveFileDirectory);
    }

    /// <summary>
    /// Checks if a data save file exists and deletes it if it does.
    /// </summary>
    public void ClearDataSave()
    {
        if (File.Exists(dataSaveFileDirectory))
            File.Delete(dataSaveFileDirectory);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    private static string SAVE_PATH = Application.persistentDataPath + "save.save";
    public static void SaveGame(SaveData saveData)
    {
        BinaryFormatter binaryFormater = new BinaryFormatter();
        FileStream file;
        try
        {
            file = File.Create(SAVE_PATH);
        }
        catch
        {
            Debug.LogError("Unable to open " + SAVE_PATH);
            return;
        }

        binaryFormater.Serialize(file, saveData);

        Debug.Log("Game Save");
        file.Close();
    }

    public static SaveData LoadGame()
    {
        SaveData loadedData = new SaveData();

        if(File.Exists(SAVE_PATH) == true)
        {
            FileStream file;
            BinaryFormatter binaryFormater = new BinaryFormatter();
            try
            {
                file = File.Open(SAVE_PATH, FileMode.Open);
            }
            catch
            {
                Debug.LogError("Unable to open " + SAVE_PATH);
                return loadedData;
            }

            loadedData = (SaveData)binaryFormater.Deserialize(file);
        }

        return loadedData;
    }
}

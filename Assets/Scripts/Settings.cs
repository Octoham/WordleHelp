using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public bool extendedWordList;




    public void savePlayer()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string savePath = path() + "/settings";
        Debug.Log(path());
        FileStream stream = new FileStream(savePath, fileMode());

        Settings data = this;

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public FileMode fileMode()
    {
        if (check())
        {
            File.Delete(path() + "/settings");
        }
        return FileMode.CreateNew;
    }
    public bool check()
    {
        return File.Exists(path() + "/settings");
    }
    public string path()
    {
        return Application.persistentDataPath;
    }
    public Settings loadPlayer()
    {
        string savePath = path() + "/settings";
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            Settings data = formatter.Deserialize(stream) as Settings;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath);
            return null;
        }
    }
}

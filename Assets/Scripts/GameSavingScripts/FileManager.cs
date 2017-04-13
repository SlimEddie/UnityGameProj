﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileManager
{
    public static void Save<T>(T obj, string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        FileStream file = File.Exists(filePath) ? file = File.Open(filePath, FileMode.Open) : file = File.Open(filePath, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, obj);
        file.Close();
    }

    public static T Load<T>(string fileName)
    {
        string filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            T data = (T)bf.Deserialize(file);
            file.Close();
            return data;
        }
        throw new System.Exception("Save file wasn't found!");
    }
}
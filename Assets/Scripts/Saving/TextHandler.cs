using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class TextHandler
{
    private const string SAVE_FILE_NAME = "Saves";
    private const string JSON_SAVE_EXT = ".json";
    private const string BASEDIR = "Assets/Saves/";

    public static string ConvertToJSON<D, T, K>(D dictionnary) where D: IDictionary<T, K> {
        return JsonUtility.ToJson(dictionnary);
    }

    public static D ParseFromJSON<D, T, K>(string json) where D: IDictionary<T, K> { 
        return JsonUtility.FromJson<D>(json);
    }

    public static D ReadFromJSON<D, T, K>(string directory = SAVE_FILE_NAME) where D : IDictionary<T, K> {
        return ParseFromJSON<D, T, K>(File.ReadAllText(BASEDIR + directory + JSON_SAVE_EXT));
    }

    public static void WriteToJSON<D, T, K>(D dictionnary, string directory = SAVE_FILE_NAME) where D : IDictionary<T, K> {
        File.WriteAllText(BASEDIR + directory + JSON_SAVE_EXT, ConvertToJSON<D, T, K>(dictionnary));
    }
}

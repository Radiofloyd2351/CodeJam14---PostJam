using System.Collections.Generic;
using UnityEngine;

public static class TextHandler
{ 
    public static string ConvertToJSON<D, T, K>(D dictionnary) where D: IDictionary<T, K> {
        return JsonUtility.ToJson(dictionnary);
    }

    public static D ParseFromJSON<D, T, K>(string json) where D: IDictionary<T, K> { 
        return JsonUtility.FromJson<D>(json);
    }
}

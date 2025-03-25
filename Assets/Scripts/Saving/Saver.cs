using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Saver : MonoBehaviour
{
    public Dictionary<string, SavesContainer> saveDict = new();

    public static Saver instance;
    // Start is called before the first frame update
    public void Fetch() {
        if (!saveDict.ContainsKey("save")) {
            saveDict.Add("save", new SavesContainer());
        }
        saveDict = TextHandler.ReadFromJSON<Dictionary<string, SavesContainer>, string, SavesContainer>();
        Debug.Log(saveDict["save"]);
    }

    public void Save() {
        Debug.Log(Saver.instance.saveDict["save"].inventory.instruments[0]);
        TextHandler.WriteToJSON<Dictionary<string, SavesContainer>, string, SavesContainer>(saveDict);
    }

    public void Start() {
        instance = this;
        Fetch();
    }
}

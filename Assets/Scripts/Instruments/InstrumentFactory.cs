using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstrumentFactory : MonoBehaviour {

    public static InstrumentFactory instance;
    private Dictionary<Instrument, AbsInstrument> allInstruments;

    public void Start() {
        allInstruments = new();
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private AbsInstrument CreateInstrument(Instrument type) {
        Type classType = DefaultValues.GetClassType(type);
        AbsInstrument newInstrument = (AbsInstrument)gameObject.AddComponent(classType);
        Debug.Log(type.ToString());
        if (!Saver.instance.saveDict["save"].inventory.instruments.Contains(type.ToString())) Saver.instance.saveDict["save"].inventory.instruments.Add(type.ToString());
        Debug.Log(Saver.instance.saveDict["save"].inventory.instruments);
        allInstruments.Add(type, newInstrument);
        return newInstrument;
    }

    public AbsInstrument GetInstrument(Instrument type) {
        if (allInstruments.ContainsKey(type)) { 
            return allInstruments[type]; 
        }
        else { 
            return CreateInstrument(type); 
        }
    }

    public void InstantiateInstruments() {
        foreach (string instrument in Saver.instance.saveDict["save"].inventory.instruments) {
           if(Enum.TryParse(instrument, out Instrument enumValue)) {
                GetInstrument(enumValue);
           }
        }
    }
}

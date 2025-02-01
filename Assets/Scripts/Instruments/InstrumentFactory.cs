using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentFactory : MonoBehaviour {

    public static InstrumentFactory instance;

    public void Start() {
        allInstruments = new();
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void FillDict() {
        if (unlocks == null) {
            unlocks = new();
            foreach (Instrument type in DefaultValues.instrumentClassTypes.Keys) {
                unlocks.Add(type, false);
            }
        }
    }

    private Dictionary<Instrument, AbsInstrument> allInstruments;
    private Dictionary<Instrument, bool> unlocks;

    private AbsInstrument CreateInstrument(Instrument type) {
        Debug.Log(type);
        System.Type classType = DefaultValues.GetClassType(type);
        Debug.Log(classType);
        AbsInstrument newInstrument = (AbsInstrument)gameObject.AddComponent(classType);
        unlocks[type] = true;
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
        foreach (KeyValuePair<Instrument, bool> unlock in unlocks) {
            if (unlock.Value) {
                GetInstrument(unlock.Key);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private TKey keyType;
    [SerializeField] private List<TValue> values = new List<TValue>();
    [SerializeField] private string newKeyString = "";
    [SerializeField] private int newKeyInt = 0;
    [SerializeField] private Color newKeyColor = new Color32(0,0,0,0);
    [SerializeField] private Level newKeyLevel = Level.Blank;
    [SerializeField] private Instrument newKeyInstrument = Instrument.Launch;
    [SerializeField] private string error = "Error";
    public void OnBeforeSerialize() {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> kvp in this) {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }

    }

    public void OnAfterDeserialize() {
        this.Clear();
        if (keys.Count != values.Count) {
            Debug.LogError("Keys and values count mismatch.");
            return;
        }

        for (int i = 0; i < keys.Count; i++) {
            this[keys[i]] = values[i];
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    [SerializeField] private string newKey;
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

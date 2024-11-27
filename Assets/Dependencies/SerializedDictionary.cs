using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<K, T> : ISerializationCallbackReceiver
{
    [SerializeField]
    private K[] keys;
    [SerializeField]
    private T[] values;
    private Dictionary<K, T> _dictionary = new();

    public T this[K key] {
        get => _dictionary[key];
        set => _dictionary[key] = value;

    }

    public void OnBeforeSerialize() {
    }

    public void OnAfterDeserialize() {
        for (int i = 0; i < keys.Length; i++) {
            _dictionary.Add(keys[i], values[i]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefaultValues : MonoBehaviour {

    //Passes OK

    public List<Type> types;


    [SerializeField] private SerializedDictionary<Instrument, AbsInstrument> instrumentTypeTemplates;



    public static Dictionary<Instrument, Type> instrumentClassTypes = new();

    public static Type GetClassType (Instrument type) { return instrumentClassTypes[type]; }

    [SerializeField] private GameObject playerObj;

    public static GameObject player;
    public static GameObject grid;
    public GameObject gridObj;

    public static Vector2 defaultPos = new Vector2(0f, 0f);

    private static Instrument _currentInstrument;
    private static Instrument _lastInstrument;

    public static Type Current { get { return instrumentClassTypes[_currentInstrument]; } }
    public static Type Last { get { return instrumentClassTypes[_lastInstrument]; } }


    public void Start() {
        player = playerObj;
        grid = gridObj;
        foreach (KeyValuePair<Instrument, AbsInstrument> type in instrumentTypeTemplates) {
            instrumentClassTypes.Add(type.Key, type.Value.GetType());
            Destroy(type.Value);
        }
        gameObject.transform.position = PlayerStats.playerPos;
        InstrumentFactory.instance.FillDict();
        InstrumentFactory.instance.GetInstrument(Instrument.None).Equip();
        InstrumentFactory.instance.InstantiateInstruments();
    }
}

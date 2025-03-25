using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefaultValues : MonoBehaviour {

    //Passes OK

    public List<Type> types;


    public SerializedDictionary<Instrument, InstrumentInfo> instrumentInfoTypeTemplates;

    public Dictionary<Instrument, Type> instrumentClassTypes = new();

    public Type GetClassType (Instrument type) { return instrumentClassTypes[type]; }
    public Type GetInfoType(Instrument type) { return instrumentClassTypes[type]; }

    public static DefaultValues instance;

    [SerializeField] private GameObject playerObj;

    public static GameObject player;
    public static PlayerStats playerStats;
    public static GameObject grid;
    public GameObject gridObj;

    public static Vector2 defaultPos = new Vector2(0f, 0f);

    private static Instrument _currentInstrument;
    private static Instrument _lastInstrument;

    public Type Current { get { return instrumentClassTypes[_currentInstrument]; } }
    public Type Last { get { return instrumentClassTypes[_lastInstrument]; } }


    public void Start() {
        instance = this;
        player = playerObj;
        playerStats = playerObj.GetComponent<PlayerStats>();
        grid = gridObj;
        foreach (KeyValuePair<Instrument, InstrumentInfo> type in instrumentInfoTypeTemplates) {
            instrumentClassTypes.Add(type.Key, type.Value.Type);
            Debug.Log(type.Value.Type);
        }
        gameObject.transform.position = PlayerInfos.playerPos;
        InstrumentFactory.instance.SwitchInstrument(Instrument.None);
    }
}

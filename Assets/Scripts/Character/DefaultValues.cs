using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class DefaultValues : MonoBehaviour {

    //Passes OK

    public List<Type> types;


    public SerializedDictionary<Instrument, InstrumentInfo> instrumentInfoTypeTemplates;

    public Dictionary<Instrument, Type> instrumentClassTypes = new();

    public Type GetClassType (Instrument type) { return instrumentClassTypes[type]; }
    public InstrumentInfo GetInfoType(Instrument type) { return instrumentInfoTypeTemplates[type]; }

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
        List<Instrument> alphabeticalTypes = new();
        foreach (KeyValuePair<Instrument, InstrumentInfo> type in instrumentInfoTypeTemplates) {
            alphabeticalTypes.Add(type.Key);
        }

        alphabeticalTypes.Sort(delegate (Instrument x, Instrument y) {
            return x.ToString().CompareTo(y.ToString());
        });

        int i = 0;
        foreach  (Instrument type in alphabeticalTypes) {
            var allTypes = Assembly.GetAssembly(typeof(AbsInstrument))
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(AbsInstrument)) && !t.IsAbstract)
            .ToList();

            string typeName = allTypes[i].AssemblyQualifiedName;
            Type classType;
            if (string.IsNullOrEmpty(typeName)) {
                classType = null;
            } else {
                classType = Type.GetType(typeName);
            }
            Debug.Log(typeName);

            instrumentClassTypes.Add(type, classType);
            Debug.Log(classType);

            i++;
        }
        gameObject.transform.position = PlayerInfos.playerPos;
        InstrumentFactory.instance.InstantiateInstruments();
        Inventory.instance.Build();
    }
}

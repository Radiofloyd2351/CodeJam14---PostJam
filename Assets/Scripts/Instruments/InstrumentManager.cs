using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentManager : MonoBehaviour {

    public static InstrumentManager instance;

    [SerializeField] private SerializedDictionary<Instrument, GameObject> gridDictionnary;
    [SerializeField] private GameObject mask;

    private void Start() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public GameObject GetGrid(Instrument type) {
        return gridDictionnary[type];
    }
    public GameObject GetMask() {
        return mask;
    }
}
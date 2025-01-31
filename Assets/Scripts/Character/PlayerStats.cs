using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    //Passes Chill

    public static Dictionary<Instrument, bool> collected;
    public static Vector3 playerPos;

    private static PlayerStats instance;

    public static bool transitionning;

    public static Instrument heldInstrument;


    public static GameObject player;

    public void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Created Instance");
            if (collected == null) {
                Debug.Log(DefaultValues.staticDefaultCollection[0]);
                collected = DefaultValues.staticDefaultCollection;
            }
        } else {
            Destroy(gameObject);
        }
    }
}

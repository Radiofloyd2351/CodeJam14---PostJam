using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static Vector3 playerPos;

    private static PlayerStats instance;

    public static bool transitionning;

    public static Instrument heldInstrument;


    public static GameObject player;

    public void Awake() {
        if (instance == null) {
            player = DefaultValues.player;
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Created Instance");
        } else {
            Destroy(gameObject);
        }
    }
}

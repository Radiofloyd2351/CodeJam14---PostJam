using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfos : MonoBehaviour {

    public static Vector3 playerPos;

    private static PlayerInfos instance;

    public static bool transitionning;

    public static Instrument heldInstrument;


    public static GameObject player;

    public void Awake() {
        if (instance == null) {
            player = DefaultValues.player;
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}

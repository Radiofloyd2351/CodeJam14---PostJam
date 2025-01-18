using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankZone : MonoBehaviour

{
    public GameObject victory;

    // public static string zoneName;


    public void Play() {
        if (Progression.unlocks[Level.Hell]) {
            victory.SetActive(true);
        }
    }
}


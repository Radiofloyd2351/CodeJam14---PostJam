using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankZone : MonoBehaviour

{

    [SerializeField]
    private MultiSourcePlayer _player;


    public GameObject victory;

    // public static string zoneName;


    public void Play() {
        _player.Play();
        if (Progression.unlocks[Level.Hell]) {
            victory.SetActive(true);
        }
    }

    public void Stop() {
        _player.Stop();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySoundManager : MonoBehaviour
{
    const string BASE_PATH = "event:/Gameplay/";

    private FMOD.Studio.EventInstance walkSound;

    public static GameplaySoundManager instance;
    public void Awake() {


        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            walkSound = FMODUnity.RuntimeManager.CreateInstance(BASE_PATH + "Walk");
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayWalkSound() {
        walkSound.start();
    }


}

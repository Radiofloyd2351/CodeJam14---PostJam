using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySoundManager : MonoBehaviour
{

    private FMOD.Studio.EventInstance walkSound;

    public FMODUnity.EventReference walkRef;

    public static GameplaySoundManager instance;
    public void Awake() {


        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            walkSound = FMODUnity.RuntimeManager.CreateInstance(walkRef);
        } else {
            Destroy(gameObject);
        }
    }

    public void PlayWalkSound() {
        walkSound.start();
    }


}

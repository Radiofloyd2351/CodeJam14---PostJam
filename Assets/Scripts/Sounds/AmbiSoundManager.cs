using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbiSoundManager : MonoBehaviour
{
    const string BASE_PATH = "event:/Ambient/";

    private FMOD.Studio.EventInstance ambienceSound;
    private FMOD.Studio.EventInstance musicSound;

    public static AmbiSoundManager instance;
    public void Awake()
    {


        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ambienceSound = FMODUnity.RuntimeManager.CreateInstance(BASE_PATH + "Amb");
            musicSound = FMODUnity.RuntimeManager.CreateInstance(BASE_PATH + "Music");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBaseAmbience()
    {
        ambienceSound.start();
    }

    public void PlayMusicAmbience()
    {
        musicSound.start();
    }

}

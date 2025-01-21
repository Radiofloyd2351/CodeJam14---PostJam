using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ZonePlayer : MonoBehaviour
{
    public FMODUnity.EventReference musicRef;
    private FMOD.Studio.EventInstance musicInstance;

    private void Awake() {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicRef);
        musicInstance.start();
    }

    public void addBlankLayer(Level zoneName) {

        switch (zoneName) {
            case Level.Tech:
                musicInstance.setParameterByName("TechEnable", 1);
                break;
            case Level.Nature:
                musicInstance.setParameterByName("NatureEnable", 1);
                break;
            case Level.Hell:
                musicInstance.setParameterByName("HellEnable", 1);
                musicInstance.setParameterByName("MedleyFinalEnable", 1);
                break;
        }
    }

    public void SetZoneTrack(Level zoneName) {
        switch (zoneName) 
        {
            case Level.Blank:
                musicInstance.setParameterByNameWithLabel("Level", "Blank");
                Debug.Log("blank");
                break;
            case Level.Nature:
                musicInstance.setParameterByNameWithLabel("Level", "Nature");
                Debug.Log("nature");
                break;
            case Level.Hell:
                musicInstance.setParameterByNameWithLabel("Level", "Hell");
                Debug.Log("hell");
                break;
            case Level.Tech:
                musicInstance.setParameterByNameWithLabel("Level", "Techno");
<<<<<<< HEAD
                Debug.Log("tech");
=======
>>>>>>> origin/soundRework
                break;
        }
       
    }
      

}



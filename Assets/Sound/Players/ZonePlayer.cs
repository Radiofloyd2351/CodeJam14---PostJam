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

    public void SetZoneTrack(Level zoneName) {
        switch (zoneName) 
        {
            case Level.Blank:
                musicInstance.setParameterByNameWithLabel("Level", "Blank");
                break;
            case Level.Nature:
                musicInstance.setParameterByNameWithLabel("Level", "Nature");
                break;
            case Level.Hell:
                musicInstance.setParameterByNameWithLabel("Level", "Hell");
                break;
            case Level.Tech:
                musicInstance.setParameterByNameWithLabel("Level", "Tech");
                break;
        }
       
    }
      

}



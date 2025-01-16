using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class MultiSourcePlayer : MonoBehaviour
{
    public AudioSource tech;
    public AudioSource hell;
    public AudioSource nature;
    public AudioSource end;
    private AudioSource lastSource;
    private Dictionary<Level, AudioSource> sources = new();

    public void Start() {
        sources.Add(Level.Blank, null);
        sources.Add(Level.Tech, tech);
        sources.Add(Level.Nature, nature);
        sources.Add(Level.Hell, hell);
    }
    public void StartSource(Level level) {
        if (sources[level] != null) {
            sources[level].Pause();
            if (sources[level] != lastSource) {
                sources[level].timeSamples = lastSource.timeSamples;
            }
            lastSource = sources[level];
            sources[level].Play();
        }
    }


    public void startEnd() {
        end.Pause();    
        end.timeSamples = tech.timeSamples;
        end.Play(); 

    }

    public void Play() {
        StartSource(Progression.lastUnlock);
        if (Progression.unlocks[Level.Tech] && Progression.unlocks[Level.Nature] && Progression.unlocks[Level.Hell]) {
            startEnd();
        }
    }

    public void Stop() {
        tech.Pause();
        hell.Stop();
        nature.Stop();
        end.Stop();
    }


}


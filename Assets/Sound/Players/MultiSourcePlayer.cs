using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class MultiSourcePlayer : MonoBehaviour
{
    private AudioSource lastSource;
    [SerializeField] private SerializedDictionary<Level, AudioSource> sources;

    public void StartSource(Level level) {
        if (sources[level] != null && lastSource != null) {
            sources[level].Pause();
            if (sources[level] != lastSource) {
                Debug.Log(sources[level]);
                sources[level].timeSamples = lastSource.timeSamples;
            }
            lastSource = sources[level];
            sources[level].Play();
        }
    }


    public void startEnd() {
        sources[Level.Blank].Pause();
        sources[Level.Blank].timeSamples = sources[Level.Tech].timeSamples;
        sources[Level.Blank].Play(); 

    }

    public void Play() {
        StartSource(Progression.lastUnlock);
        if (Progression.unlocks[Level.Tech] && Progression.unlocks[Level.Nature] && Progression.unlocks[Level.Hell]) {
            startEnd();
        }
    }

    public void Stop() {
        sources[Level.Tech].Pause();
        sources[Level.Nature].Stop();
        sources[Level.Hell].Stop();
        sources[Level.Blank].Stop();
    }


}


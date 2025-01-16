using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonePlayer : MonoBehaviour
{
    AudioSource _source;
    [SerializeField]
    BlankZone _blankZone;

    private void Awake() {  
        _source = GetComponent<AudioSource>();
    }

    public void playZoneTrack(string zoneName) {
        if (zoneName == "Blank") {
            _blankZone.Stop();
            _blankZone.Play();
        }
        else {
            _blankZone.Stop();
        }
        string currentClip = ZoneContainer.getClipName(_source.clip);
        if (currentClip != null) {
            ZoneContainer.SetOffset(currentClip, _source.timeSamples);
        }
        _source.Pause();
        _source.clip = ZoneContainer.GetClip(zoneName);
        _source.timeSamples = ZoneContainer.GetOffset(zoneName);
        _source.Play();
    }

}



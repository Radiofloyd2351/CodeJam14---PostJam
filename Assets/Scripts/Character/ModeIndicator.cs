using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeIndicator : MonoBehaviour
{

    //Passes Chill

    public Sprite active;
    public Sprite inactive;
    public Instrument instrument;

    public void Click() {
        InstrumentFactory.instance.SwitchInstrument(instrument);
    }

    public void Enable() {
        GetComponent<Image>().sprite = active;
    }
    public void Disable() {
        GetComponent<Image>().sprite = inactive;
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeIndicator : MonoBehaviour
{

    //Passes Chill

    public Sprite active;
    public Sprite inactive;
    [SerializeField]
    private Instrument instrument;
    [SerializeField]
    private KeyCode key;

    public void Click() {
        InstrumentFactory.instance.GetInstrument(instrument).Equip();
    }

    public void Enable() {
        GetComponent<Image>().sprite = active;
    }
    public void Disable() {
        GetComponent<Image>().sprite = inactive;
    }



}

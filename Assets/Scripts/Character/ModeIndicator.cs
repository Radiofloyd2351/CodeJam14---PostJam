using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeIndicator : MonoBehaviour, IPointerEnterHandler/*, IPointerExitHandler*/ {
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

    public void OnPointerEnter(PointerEventData eventData) {
        if (Inventory.instance.selectedInstrument != null) {
            InstrumentFactory.instance.ReplaceInstrument(Inventory.instance.selectedInstrument.info.type, instrument);
            Inventory.instance.selectedInstrument.OnEndDrag(null);
        }
    }
    /*public void OnPointerExit(PointerEventData eventData) {
    }*/


}

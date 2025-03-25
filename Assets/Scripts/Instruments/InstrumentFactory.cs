using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstrumentFactory : MonoBehaviour {

    public static InstrumentFactory instance;
    private AbsInstrument heldInstrument;

    public void Start() {
        instance = this;
    }


    public void SwitchInstrument(Instrument type) {
        Type classType = DefaultValues.instance.GetClassType(type);
        if (heldInstrument != null) {
            if (Saver.instance.saveDict["save"].inventory.heldInstruments.Count >= 3 && !Saver.instance.saveDict["save"].inventory.heldInstruments.Contains(type.ToString())) {
                Saver.instance.saveDict["save"].inventory.heldInstruments.Remove(heldInstrument.type.ToString());
            }
            UnequipCurrent();
        }
        heldInstrument = (AbsInstrument)gameObject.AddComponent(classType);
        Debug.Log(classType);
        if (!Saver.instance.saveDict["save"].inventory.heldInstruments.Contains(type.ToString())) Saver.instance.saveDict["save"].inventory.heldInstruments.Add(type.ToString());
        if (!Saver.instance.saveDict["save"].inventory.instruments.Contains(type.ToString())) Saver.instance.saveDict["save"].inventory.instruments.Add(type.ToString());
        heldInstrument.Equip();
    }

    public void UnequipCurrent() {
        heldInstrument.Unequip();
        Destroy(heldInstrument);
    }
}

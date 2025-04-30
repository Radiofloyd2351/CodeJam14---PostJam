using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstrumentFactory : MonoBehaviour {

    public static InstrumentFactory instance;
    [SerializeField] private SerializedDictionary<Instrument, ModeIndicator> indicators;
    public List<Instrument> positions = new();
    private AbsInstrument heldInstrument;
    [SerializeField] private GameObject indicatorTemplate;
    [SerializeField] private Transform indicatorContainer;
    [SerializeField] private float modifier;

    public void Start() {
        instance = this;
    }

    public void InstantiateInstruments() {
        foreach (string instrumentText in Saver.instance.saveDict["save"].inventory.heldInstruments) {
            if (Enum.TryParse(instrumentText, out Instrument type)) {
                SwitchInstrument(type);
            }
        }
    }

    public void ReplaceInstrument(Instrument type, Instrument lastType) {
        SwitchInstrument(lastType);
        SwitchInstrument(type);
    }

    public void SwitchInstrument(Instrument type) {
        if (heldInstrument && type == heldInstrument.type) {
            return;
        }
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;

        // Not holding heldInstrument anymore
        if (heldInstrument != null) {
            indicators[heldInstrument.type].GetComponent<ModeIndicator>().Disable();
            heldInstrument.Unequip();
            Destroy(heldInstrument);
            Inventory.instance.instruments[heldInstrument.type].Unequip();
        }

        // Gain the instrument
        if (!heldInstruments.Contains(type.ToString())) {
            GainInstrument(type);
        }

        // Hold designated Instrument
        indicators[type].Enable();
        Debug.Log(type);
        Type classType = DefaultValues.instance.GetClassType(type);
        Debug.Log(classType);
        heldInstrument = (AbsInstrument)gameObject.AddComponent(classType);
        Debug.Log(heldInstrument);
        heldInstrument.Equip();
    }
    public void SwitchInstrument(int index) {
        Debug.Log("AAAA");
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
        if (positions.Count > index) {
            SwitchInstrument(positions[index]);
        }
    }
    public void GainInstrument(Instrument type) {
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
        // Add to inventory if not there
        if (!Saver.instance.saveDict["save"].inventory.instruments.Contains(type.ToString())) {
            Saver.instance.saveDict["save"].inventory.instruments.Add(type.ToString());
            Inventory.instance.Build();
        }
        // Remove last held item if too much
        bool delete = heldInstruments.Count >= 4;
        if (delete) {
            positions[positions.IndexOf(heldInstrument.type)] = type;
            heldInstruments.Remove(heldInstrument.type.ToString());
        } else {
            positions.Add(type);
        }
        heldInstruments.Add(type.ToString());
        CreateIndicator(type, delete);
    }

    private void CreateIndicator(Instrument type, bool delete) {
        ModeIndicator indicator;
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
        if (delete) {
            indicator = indicators[heldInstrument.type].GetComponent<ModeIndicator>();
            indicators.Add(type, indicators[heldInstrument.type]);
            indicators.Remove(heldInstrument.type);
        } else {
            GameObject indicatorObj = Instantiate(indicatorTemplate, indicatorContainer);
            indicatorObj.transform.position += new Vector3(indicators.Count * modifier, 0f, 0f);
            indicator = indicatorObj.GetComponent<ModeIndicator>();
            indicators.Add(type, indicator);
        }
        indicator.instrument = type;
        indicator.active = DefaultValues.instance.GetInfoType(type).activeIndicator;
        indicator.inactive = DefaultValues.instance.GetInfoType(type).inactiveIndicator;
    }
}

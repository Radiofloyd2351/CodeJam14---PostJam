using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InstrumentFactory : MonoBehaviour {

    public static InstrumentFactory instance;
    [SerializeField] private SerializedDictionary<Instrument, GameObject> indicators; 
    private AbsInstrument heldInstrument;
    [SerializeField] private GameObject indicatorTemplate;
    [SerializeField] private Transform indicatorContainer;
    [SerializeField] private float modifier;

    public void Start() {
        instance = this;
    }

    public void InstantiateInstruments() {
        foreach (string instrumentText in Saver.instance.saveDict["save"].inventory.heldInstruments) {
            if (Enum.TryParse(instrumentText, out Instrument instrument)) {
                SwitchInstrument(instrument);
            }
        }
    }

    public void SwitchInstrument(Instrument type) {
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
        Type classType = DefaultValues.instance.GetClassType(type);
        if (heldInstrument != null) {
            if (indicators.Count >= 4 && !heldInstruments.Contains(type.ToString())) {
                Saver.instance.saveDict["save"].inventory.heldInstruments.Remove(heldInstrument.type.ToString());
                ReplaceIndicator(type, heldInstrument.type);
            } else {
                if (!heldInstruments.Contains(type.ToString())) {
                    GainInstrument(type);
                }
                CreateIndicator(type);
            }
            indicators[heldInstrument.type].GetComponent<ModeIndicator>().Disable();
            UnequipCurrent();
        } else {
            GainInstrument(type);
            CreateIndicator(type);
        }

        heldInstrument = (AbsInstrument)gameObject.AddComponent(classType);
        if (!heldInstruments.Contains(type.ToString())) Saver.instance.saveDict["save"].inventory.heldInstruments.Add(type.ToString());
        if (!Saver.instance.saveDict["save"].inventory.instruments.Contains(type.ToString())) Saver.instance.saveDict["save"].inventory.instruments.Add(type.ToString());
        heldInstrument.Equip();
        indicators[type].GetComponent<ModeIndicator>().Enable();
    }

    public void SwitchInstrument(int index) {
        List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
        if (heldInstruments.Count > index) {
            if (Enum.TryParse(heldInstruments[index], out Instrument instrument)) {
                SwitchInstrument(instrument);
            }
        }
    }

    private void CreateIndicator(Instrument type) {
        if (!indicators.ContainsKey(type)) {
            List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
            GameObject newIndicator = Instantiate(indicatorTemplate, indicatorContainer);
            newIndicator.transform.position += new Vector3(indicators.Count * modifier, 0f, 0f);
            ModeIndicator script = newIndicator.GetComponent<ModeIndicator>();
            script.instrument = type;
            script.active = DefaultValues.instance.GetInfoType(type).activeIndicator;
            script.inactive = DefaultValues.instance.GetInfoType(type).inactiveIndicator;
            indicators.Add(type, newIndicator);
        }
    }
    private void ReplaceIndicator(Instrument type, Instrument lastType) {
        if (!indicators.ContainsKey(type)) {
            ModeIndicator script = indicators[lastType].GetComponent<ModeIndicator>();
            List<string> heldInstruments = Saver.instance.saveDict["save"].inventory.heldInstruments;
            script.instrument = type;
            script.active = DefaultValues.instance.GetInfoType(type).activeIndicator;
            script.inactive = DefaultValues.instance.GetInfoType(type).inactiveIndicator;
        }
    }

    public void GainInstrument(Instrument type) {
        if (!Saver.instance.saveDict["save"].inventory.instruments.Contains(type.ToString())) {
            Saver.instance.saveDict["save"].inventory.instruments.Add(type.ToString());
            Inventory.instance.Build();
        }
    }

    public void UnequipCurrent() {
        heldInstrument.Unequip();
        Destroy(heldInstrument);
    }
}

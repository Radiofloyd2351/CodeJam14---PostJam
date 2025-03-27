using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drums : AbsInstrument {

    public Drums() {
        type = Instrument.Drums;
        layer = "Launch";
    }

    public override void PlaySound() {
        Debug.Log("my pen is smoother than an drums: " + type);
    }

    public override void EquipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(true);
    }

    public override void UnequipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(false);
    }

    private IEnumerator Shoot() {
        yield return null;
        Debug.Log("my pen is sharper than an drumsss: " + type);
    }
}
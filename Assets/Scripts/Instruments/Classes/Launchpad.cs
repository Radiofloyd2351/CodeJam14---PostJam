using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchpad : AbsInstrument {

    public Launchpad() {
        type = Instrument.Launch;
        layer = "Launch";
    }

    public override void PlaySound() {
        Debug.Log("my pen is smoother than an iLady: "+type);
    }

    public override void EquipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(true);
    }

    public override void UnequipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(false);
    }

    private IEnumerator Shoot() {
        yield return null;
        Debug.Log("my pen is sharper than an axe: " + type);
    }
}
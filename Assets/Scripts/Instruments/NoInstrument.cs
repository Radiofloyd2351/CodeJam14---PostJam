using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInstrument : AbsInstrument {

    public NoInstrument() {
        type = Instrument.None;
    }

    public override void PlaySound() {
        Debug.Log("my pen is smoother than an iLady: "+type);
    }

    public override void EquipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(false);
    }

    public override void UnequipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(true);
    }

    private IEnumerator Shoot() {
        yield return null;
        Debug.Log("my pen is sharper than an axe: "+type);
    }
}
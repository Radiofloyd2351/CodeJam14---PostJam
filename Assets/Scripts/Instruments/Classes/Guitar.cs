using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guitar : AbsInstrument {

    public Guitar() {
        type = Instrument.Guitar;
        layer = "Guitar";
    }
    public override void PlaySound() {
        Debug.Log("my pen is smoother than an iLady: "+type);
    }

    public override void EquipAbilities() {
        InstrumentManager.instance.GetMask().gameObject.SetActive(true);
        OnUse += Shoot;
    }

    public override void UnequipAbilities() {
        Debug.Log(InstrumentManager.instance);
        InstrumentManager.instance.GetMask().gameObject.SetActive(false);  //MUST PLACE THAT IN BLANK INSTEAD!
        OnUse -= Shoot;
    }

    private IEnumerator Shoot() {
        yield return null;
        Debug.Log("my pen is sharper than an axe: "+type);
    }


}

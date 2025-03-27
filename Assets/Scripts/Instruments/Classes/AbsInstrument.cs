using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator DefaultEventHandler();

public abstract class AbsInstrument : MonoBehaviour {

    public event DefaultEventHandler OnUse;

    [SerializeField] public RuntimeAnimatorController animController;
    [SerializeField] public Instrument type;
    [SerializeField] public string layer;
    protected InstrumentInfo data;
    //the class that plays audio if needed

    public abstract void PlaySound();
    public virtual void Equip() {
        EventHandler.instance.RunSwitchInstrument(type);
        InstrumentManager.instance.GetGrid(type).SetActive(true);
        DefaultValues.player.layer = LayerMask.NameToLayer(layer);
        PlayerInfos.heldInstrument = type;
        //animator.runtimeAnimatorController = DefaultValues.Current.controller;
        EquipAbilities();
    }
    public void Unequip() {
        if (type != Instrument.None) {
            InstrumentManager.instance.GetGrid(type).SetActive(false);
        }
        UnequipAbilities();
    }

    public virtual void EquipAbilities() { }
    public virtual void UnequipAbilities() { }

    protected bool OnRunUse() {
        if (OnUse != null) {
            if (OnUse != null) {
                foreach (DefaultEventHandler handler in OnUse.GetInvocationList()) {
                    CoroutineManager.instance.RunCoroutine(handler());
                }
            }
            return true;
        }
        return false;
    }



    public static void HoldInstrument(Instrument newInstrument) {
        
    }

}

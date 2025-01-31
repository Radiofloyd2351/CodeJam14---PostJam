using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeSwitcher : MonoBehaviour
{

    //Passes Chill

    public GameObject lastGrid;
    public GameObject mainGrid;

    public GameObject mask;

    private Animator animator;

    public bool container = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        DefaultValues.HoldInstrument(Instrument.None);
        mask.SetActive(false);
    }

    public void ChangeMode(Instrument instrument) {

        EventHandler.instance.RunSwitchInstrument(instrument);

        //Changes instruments, indicators and grids
        DefaultValues.HoldInstrument(instrument);
        //Toggles Mask
        if (instrument == Instrument.None) {
            mask.SetActive(false);
        } else {
            mask.SetActive(true);
        }
        //Changes Animation,
        PlayerStats.heldInstrument = instrument;
        animator.runtimeAnimatorController = DefaultValues.Current.controller;
    }

    public void SwitchToCollected(Instrument instrument) {
        ChangeMode(instrument);
        container = false;
        Debug.Log($"Collision with { DefaultValues.Current.instrumentName} detected!");
        PlayerStats.collected[instrument] = true;
        //info.gameObject.SetActive(false);
    }
}

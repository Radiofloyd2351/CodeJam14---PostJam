using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeSwitcher : MonoBehaviour
{

    [SerializeField]
    private SerializedDictionary<Instrument , InstrumentInfo> instrumentsInfo;

    private Instrument lastInstrument;

    public GameObject lastGrid;
    public GameObject mainGrid;

    public GameObject mask;

    private Animator animator;

    public static HashSet<Instrument> collectedInstruments = new HashSet<Instrument>();
    public static Instrument currentInstrument;

    public bool container = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentInstrument = Instrument.None;
    }

    public void changeMode(Instrument instrument) {
        //Changes Animation,
        SwitchAnimator(instrument);
        //Change Grid
        if (lastGrid != null && lastGrid != mainGrid) {
            lastGrid.SetActive(false);
        }
        instrumentsInfo[instrument].grid.SetActive(true);
        lastGrid = instrumentsInfo[instrument].grid;
        //Change Indicator
        instrumentsInfo[lastInstrument].indicator.Disable();
        instrumentsInfo[instrument].indicator.Enable();
        lastInstrument = instrument;
        //Toggles Mask
        if (instrument == Instrument.None) {
            mask.SetActive(false);
        } else {
            mask.SetActive(true);
        }
    }

    private void Update()
    {
        if (!container && CameraFollow.currentZone == Level.Blank) 
        {
            container = true;
            SwitchAnimator(0);
            instrumentsInfo[lastInstrument].indicator.Disable();
            instrumentsInfo[Instrument.None].indicator.Enable();
            lastInstrument = Instrument.None;
            mask.SetActive(false);
        }
    }

    public void SwitchAnimator(Instrument instrument) {
        currentInstrument = instrument;

        animator.runtimeAnimatorController = instrumentsInfo[instrument].controller;
    }

    public void SwitchToCollected(Instrument instrument) {
        InstrumentInfo info = instrumentsInfo[instrument];
        container = false;
        Debug.Log($"Collision with {info.instrumentName} detected!");
        collectedInstruments.Add(instrument);
        SwitchAnimator(instrument);
        info.gameObject.SetActive(false);
    }
}

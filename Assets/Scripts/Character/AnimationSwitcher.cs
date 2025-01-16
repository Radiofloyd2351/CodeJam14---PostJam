using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationSwitcher : MonoBehaviour
{
    public RuntimeAnimatorController[] controllers;
    public GameObject[] instruments;
    public ModeUI[] uis;

    private Level lastLevel;


    public GameObject[] grids;
    public GameObject lastGrid;
    public GameObject mainGrid;

    public GameObject mask;

    private Animator animator;

    public static HashSet<string> collectedInstruments = new HashSet<string>();
    public static Level currentMode;

    public bool container = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentMode = Level.Blank;
    }

    public void changeMode(Level level) {

        Debug.Log("Anim BLANK");
        SwitchAnimator(level);
        if (lastGrid != null && lastGrid != mainGrid) {
            lastGrid.SetActive(false);
        }
        grids[(int)level].SetActive(true);
        lastGrid = grids[(int)level];
        uis[(int)lastLevel].Disable();
        uis[(int)level].Enable();
        lastLevel = level;
        if (level == Level.Blank) {
            mask.SetActive(false);
        } else {
            mask.SetActive(true);
        }
    }

    private void Update()
    {
        if (!container && ZoneDelimiting.zoneName == "Blank") 
        {
            container = true;
            SwitchAnimator(0);
            uis[(int)lastLevel].Disable();
            uis[0].Enable();
            lastLevel = Level.Blank;
            mask.SetActive(false);
        }
    }

    public void SwitchAnimator(Level level) {
        currentMode = level;

        animator.runtimeAnimatorController = controllers[(int)level];
    }

    public void SwitchToCollected(string name)
    {
        for (int i = 0; i < controllers.Length; i++)
        {
            GameObject instrument = instruments[i];
            if (name == instrument.name) {
                container = false;
                Debug.Log($"Collision with {instrument.name} detected!");
                collectedInstruments.Add(instrument.name);
                SwitchAnimator((Level)(i+1));
                instrument.SetActive(false);
                break;
            }
        }

    }

}

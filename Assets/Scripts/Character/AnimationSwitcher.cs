using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationSwitcher : MonoBehaviour
{
    public RuntimeAnimatorController[] controllers;
    public GameObject[] instruments;
    public ModeUI[] uis;

    private int lastIndex;


    public GameObject[] grids;
    public GameObject lastGrid;
    public GameObject mainGrid;

    public GameObject mask;

    private Animator animator;

    public static HashSet<string> collectedInstruments = new HashSet<string>();
    public static string currentMode;

    public bool container = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentMode = "Blank";
    }

    public void changeMode(int mode) {

        Debug.Log("Anim BLANK");
        SwitchAnimator(mode);
        Debug.Log(mode);
        if (lastGrid != null && lastGrid != mainGrid) {
            lastGrid.SetActive(false);
        }
        grids[mode].SetActive(true);
        lastGrid = grids[mode];
        uis[lastIndex].Disable();
        uis[mode].Enable();
        lastIndex = mode;
        if (mode == 0) {
            mask.SetActive(false);
        } else {
            mask.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Debug.Log("Anim BLANK");
            changeMode(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && collectedInstruments.Contains("Launchpad"))
        {
            Debug.Log("Anim Tech");
            changeMode(1);
        } 
        else if (Input.GetKey(KeyCode.Alpha3) && collectedInstruments.Contains("Lyre")) {
            
            Debug.Log("Anim Nature");
            changeMode(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4) && collectedInstruments.Contains("Guitar")) {
           
            Debug.Log("Anim Hell");
            changeMode(3);
        }
        else if (!container && ZoneDelimiting.zoneName == "Blank") 
        {
            container = true;
            SwitchAnimator(0);
            uis[lastIndex].Disable();
            uis[0].Enable();
            lastIndex = 0;
            mask.SetActive(false);
        }
    }

    public void SwitchAnimator(int index)
    {
        if (index >= 0 && index < controllers.Length)
        {
            switch (index)
            {
                case 0:
                    currentMode = "Blank";
                    break;
                case 1:
                    currentMode = "Tech";
                    break;
                case 2:
                    currentMode = "Nature";
                    break;
                case 3:
                    currentMode = "Hell";
                    break;
                default:
                    currentMode = "Blank";
                    break;
            }

            animator.runtimeAnimatorController = controllers[index];
        }
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
                SwitchAnimator(i+1);
                instrument.SetActive(false);
                break;
            }
        }

    }

}

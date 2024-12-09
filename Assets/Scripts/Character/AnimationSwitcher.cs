using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationSwitcher : MonoBehaviour
{
    public RuntimeAnimatorController[] controllers;
    public GameObject[] instruments;


    //grids!
    public GameObject normal;
    public GameObject tech;
    public GameObject nature;
    public GameObject hell;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1)) {
            Debug.Log("Anim BLANK");
            SwitchAnimator(0);
            tech.SetActive(false);
            mask.SetActive(false);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && collectedInstruments.Contains("Launchpad"))
        {
            Debug.Log("Anim Tech");
            SwitchAnimator(1);
            tech.SetActive(true);
            mask.SetActive(true);
        } 
        else if (Input.GetKey(KeyCode.Alpha3) && collectedInstruments.Contains("Lyre")) {
            
            Debug.Log("Anim Nature");
            SwitchAnimator(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4) && collectedInstruments.Contains("Guitar")) {
           
            Debug.Log("Anim Hell");
            SwitchAnimator(3);
        }
        else if (!container && ZoneDelimiting.zoneName == "Blank") 
        {
            container = true;
            SwitchAnimator(0);
        }
    }

    private void SwitchAnimator(int index)
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

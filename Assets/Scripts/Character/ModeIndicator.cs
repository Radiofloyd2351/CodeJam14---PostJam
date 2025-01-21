using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeIndicator : MonoBehaviour
{

    public Sprite active;
    public Sprite inactive;
    [SerializeField]
    private ModeSwitcher state;
    [SerializeField]
    private Instrument instrument;
    [SerializeField]
    private KeyCode key;

    public void Click() {
        state.ChangeMode(instrument);
        Debug.Log(instrument + " salut " + PlayerStats.collected[instrument]);
    }

    public void Enable() {
        GetComponent<Image>().sprite = active;
    }
    public void Disable() {
        GetComponent<Image>().sprite = inactive;
    }

    /*public void Update() {
        if (Input.GetKey(key)) {
            Debug.Log("Anim BLANK");
            state.changeMode(instrument);
        }
    }*/



}

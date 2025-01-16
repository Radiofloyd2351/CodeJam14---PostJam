using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeUI : MonoBehaviour
{

    public Sprite active;
    public Sprite inactive;
    [SerializeField]
    private AnimationSwitcher state;
    [SerializeField]
    private int number;

    public void Click() {
        state.changeMode(number);
        Debug.Log(number + "salut");
    }

    public void Enable() {
        GetComponent<Image>().sprite = active;
    }
    public void Disable() {
        GetComponent<Image>().sprite = inactive;
    }
}

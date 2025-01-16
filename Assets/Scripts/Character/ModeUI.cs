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
    private Level level;
    [SerializeField]
    private KeyCode key;

    public void Click() {
        state.changeMode(level);
        Debug.Log(level + " salut");
    }

    public void Enable() {
        GetComponent<Image>().sprite = active;
    }
    public void Disable() {
        GetComponent<Image>().sprite = inactive;
    }

    public void Update() {
        if (Input.GetKey(key)) {
            Debug.Log("Anim BLANK");
            state.changeMode(level);
        }
    }
}

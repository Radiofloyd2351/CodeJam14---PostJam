using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate IEnumerator ClickEventHandler();
public delegate IEnumerator InstrumentEventHandler(Instrument instrument);
public class EventHandler : MonoBehaviour
{

    public static EventHandler instance;

    private void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public event ClickEventHandler OnInterract;
    public event InstrumentEventHandler OnInstrumentSwitch;

    public bool RunInterraction() {
        if (OnInterract != null) {
            foreach (ClickEventHandler handler in OnInterract.GetInvocationList()) {
                CoroutineManager.instance.RunCoroutine(handler());
            }
            return true;
        }
        return false;
    }

    public bool RunSwitchInstrument(Instrument instrument) {
        if (OnInstrumentSwitch != null) {
            foreach (InstrumentEventHandler handler in OnInstrumentSwitch.GetInvocationList()) {
                CoroutineManager.instance.RunCoroutine(handler(instrument));
            }
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour 
{

    [SerializeField] public SerializedDictionary<Instrument, InstrumentInfo> instrumentsInfoObj;
    public static SerializedDictionary<Instrument, InstrumentInfo> instrumentsInfo;

    [SerializeField] public SerializedDictionary<Instrument, bool> defaultCollection;
    public static SerializedDictionary<Instrument, bool> staticDefaultCollection;

    public static Vector2 defaultPos = new Vector2(0f, 0f);

    private static Instrument _currentInstrument;
    private static Instrument _lastInstrument;

    public static InstrumentInfo Current { get { return instrumentsInfo[_currentInstrument]; } }
    public static InstrumentInfo Last { get { return instrumentsInfo[_lastInstrument]; } }

    public void Awake() {
        instrumentsInfo = instrumentsInfoObj;
        staticDefaultCollection = defaultCollection;
       // PlayerStats.Create();
    }

    public static void HoldInstrument(Instrument newInstrument) {
        if (instrumentsInfo != null) {
            _lastInstrument = _currentInstrument;
            _currentInstrument = newInstrument;
            if (Last.indicator != null) {
                Last.indicator.Disable();
            }
            Debug.Log(instrumentsInfo[Instrument.None]);
            Current.indicator.Enable();
            Debug.Log(Current.grid);
            if (Last.grid != null && _lastInstrument != Instrument.None) {
                Last.grid.SetActive(false);
            }
            Current.grid.SetActive(true);

        }
    }

    public void Start() {
        gameObject.transform.position = PlayerStats.playerPos;
    }
}

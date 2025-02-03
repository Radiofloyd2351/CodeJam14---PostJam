using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject button;

    [SerializeField] private Level level;
    [SerializeField] private Instrument type;

    public GameObject Ui;


    private void OnTriggerEnter2D(Collider2D collision) {
        button.SetActive(true);
        EventHandler.instance.OnInterract += PickUp;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        button.SetActive(false);
        EventHandler.instance.OnInterract -= PickUp;
    }

    private IEnumerator PickUp() {
        yield return null;
        Debug.Log(type);
        Ui.SetActive(true);
        InstrumentFactory.instance.GetInstrument(type).Equip();
        gameObject.SetActive(false);
        button.SetActive(false);
        EventHandler.instance.OnInterract -= PickUp;
    }
}

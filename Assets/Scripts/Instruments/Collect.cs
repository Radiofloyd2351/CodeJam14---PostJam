using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject button;
    private ModeSwitcher modeSwitcher;
    [SerializeField]
    private Level level;
    private InstrumentInfo info;

    public GameObject Ui;

    void Start()
    {
        modeSwitcher = player.GetComponent<ModeSwitcher>();
        info = GetComponent<InstrumentInfo>();
    }

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
        Ui.SetActive(true);
        Progression.Enter(level);
        modeSwitcher.SwitchToCollected(info.type);
        gameObject.SetActive(false);
        PlayerStats.collected[info.type] = true;
        button.SetActive(false);
        EventHandler.instance.OnInterract -= PickUp;
    }
}

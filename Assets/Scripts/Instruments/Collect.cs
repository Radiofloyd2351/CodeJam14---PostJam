using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject bag;
    public GameObject button;

    [SerializeField] private Camera cam;
    [SerializeField] private int steps;
    [SerializeField] private Level level;
    [SerializeField] private Instrument type;

    private bool interracted = false;
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
        if(!interracted) {
            interracted = true;
            EventHandler.instance.OnInterract -= PickUp;
            yield return null;
            Debug.Log(type);
            Ui.SetActive(true);
            InstrumentFactory.instance.SwitchInstrument(type);
            button.SetActive(false);


            Vector3 bagPos = cam.ScreenToWorldPoint(new Vector3(bag.transform.position.x, bag.transform.position.y, bag.transform.position.z));

            Vector3 diff = bagPos - transform.position;
            for (int i = 0; i < steps; i++) {
                transform.position += diff / steps;
                yield return new WaitForSeconds(0.01f);
            }
            gameObject.SetActive(false);
        }
    }
}

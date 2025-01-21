using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject E;
    private ModeSwitcher modeSwitcher;
    [SerializeField]
    private Level level;
    private InstrumentInfo info;

    public GameObject Ui;

    // Start is called before the first frame update
    void Start()
    {
        modeSwitcher = player.GetComponent<ModeSwitcher>();
        info = GetComponent<InstrumentInfo>();
    }


    private void OnCollisionStay2D(Collision2D collision) {
        E.SetActive(true);
        if (E.activeSelf && Input.GetKey(KeyCode.E)) {
            Ui.SetActive(true);
            Progression.Enter(level);
            modeSwitcher.SwitchToCollected(info.type);
            gameObject.SetActive(false);
            PlayerStats.collected[info.type] = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        E.SetActive(false);
    }
}

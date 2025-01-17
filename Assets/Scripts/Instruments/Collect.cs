using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject E;
    private ModeSwitcher animationSwitcher;
    [SerializeField]
    private Level level;

    public GameObject Ui;

    // Start is called before the first frame update
    void Start()
    {
        animationSwitcher = player.GetComponent<ModeSwitcher>();
    }


    private void OnCollisionStay2D(Collision2D collision) {
        E.SetActive(true);
        if (E.activeSelf && Input.GetKey(KeyCode.E)) {
            Ui.SetActive(true);
            Progression.Enter(level);
            animationSwitcher.SwitchToCollected(GetComponent<InstrumentInfo>().type);
            gameObject.SetActive(false);

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        E.SetActive(false);
    }
}

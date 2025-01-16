using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public GameObject player;
    public GameObject E;
    private AnimationSwitcher animationSwitcher;

    public GameObject Ui;

    // Start is called before the first frame update
    void Start()
    {
        animationSwitcher = player.GetComponent<AnimationSwitcher>();
    }


    private void OnCollisionStay2D(Collision2D collision) {
        E.SetActive(true);
        if (E.activeSelf && Input.GetKey(KeyCode.E)) {
            Ui.SetActive(true);
            switch (gameObject.name) {
                case "Launchpad":
                    Progression.EnterTech();
                    Debug.Log("Started!!!!!!!!!!!!!");
                    break;
                case "Lyre":
                    Progression.EnterNature();
                    break;
                case "Guitar":
                    Progression.EnterHell();
                    break;
            }
            animationSwitcher.SwitchToCollected(gameObject.name);
            gameObject.SetActive(false);

        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        E.SetActive(false);
    }
}

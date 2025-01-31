using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchpadGame : MonoBehaviour
{


    public static bool win = false;

    [SerializeField] private Sprite red;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite blue;

    public int score = 0;

    [SerializeField] private GameObject E;

    [SerializeField] private bool isUnlockNature = false;

    public AudioSource source;
    public AudioClip clip;

    //[SerializeField] private InputSystem inputSystem;


    private void OnCollisionStay2D(Collision2D collision) {
        if (PlayerStats.heldInstrument == Instrument.Launch)
        {
            E.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                SwitchScenes.SwitchScenes1();
            }
        } else {
            E.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        E.SetActive(false);
    }


    public void EndGame() {
        SwitchScenes.SwitchScenes2();
        if (isUnlockNature) {
            Progression.Open(Level.Nature);
        } else
            Progression.Win(Level.Tech);
    }

    public void ChangeColor(GameObject button) {

        if (RecordingContainer.recordings.ContainsKey(Instrument.Launch)) {
            source.clip = RecordingContainer.recordings[Instrument.Launch].internalClip;
            source.Stop();
            source.timeSamples = RecordingContainer.recordings[Instrument.Launch].offset;
            source.Play();
        }

        LaunchColor color = button.GetComponent<LaunchPadButton>().color;
        Debug.Log(color);
        switch (color) {
            case LaunchColor.Red:
                button.GetComponent<LaunchPadButton>().color = LaunchColor.Green;
                button.GetComponent<SpriteRenderer>().sprite = green;
                score++;
                break;
            case LaunchColor.Green:
                button.GetComponent<LaunchPadButton>().color = LaunchColor.Blue;
                button.GetComponent<SpriteRenderer>().sprite = blue;
                score--;
                break;
            case LaunchColor.Blue:
                button.GetComponent<LaunchPadButton>().color = LaunchColor.Red;
                button.GetComponent<SpriteRenderer>().sprite = red;
                break;
            default:
                break;
           
        }
        if (score >= 9) {
            EndGame();
        }
    }



}

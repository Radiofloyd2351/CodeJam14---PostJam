using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchpadGame : MonoBehaviour {


    public static bool win = false;

    [SerializeField] private Sprite red;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite blue;

    public int score = 0;

    [SerializeField] private GameObject button;

    [SerializeField] private bool isUnlockNature = false;

    public AudioSource source;
    public AudioClip clip;

    //[SerializeField] private InputSystem inputSystem;


    private void OnCollisionEnter2D(Collision2D collision) {
        if (PlayerStats.heldInstrument == Instrument.Launch) {
            button.SetActive(true);
            EventHandler.instance.OnInterract += ActivateGame;

        } else {
            EventHandler.instance.OnInstrumentSwitch += ActivateButton;
        }
    }

    private IEnumerator ActivateButton(Instrument instrument) {
        if (instrument == Instrument.Launch) {
            button.SetActive(true);
            EventHandler.instance.OnInterract += ActivateGame;
        } else {
            button.SetActive(false);
            EventHandler.instance.OnInterract -= ActivateGame;
        }
        yield return null;
    }

    private IEnumerator ActivateGame() {
        SwitchScenes.SwitchScenes1();
        yield return null;
    }

    private void OnCollisionExit2D(Collision2D collision) {
        button.SetActive(false);
        EventHandler.instance.OnInterract -= ActivateGame;
        EventHandler.instance.OnInstrumentSwitch -= ActivateButton;
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

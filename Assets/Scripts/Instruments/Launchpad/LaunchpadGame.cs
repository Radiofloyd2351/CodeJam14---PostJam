using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchpadGame : MonoBehaviour
{
    public Sprite red;
    public Sprite green;
    public Sprite blue;

    public int score = 0;

    public Transform gamePosition;

    public GameObject player;

    public GameObject door; 
    public static bool isPlaying = false;

    public Transform computerLocation;
    public GameObject E;

    public static bool win = false;
    private Vector2 returnPosition;
    public bool isUnlockNature = false;

    public AudioSource source;
    public AudioClip clip;

    public InputSystem inputSystem;


    public void Update()
    {
        if (!isPlaying && ModeSwitcher.currentInstrument == Instrument.Launch && ModeSwitcher.collectedInstruments.Contains(Instrument.Launch))
        {
            float distance = Vector3.Distance(computerLocation.position, player.transform.position);
            if (distance <= 1.5f)
            {
                returnPosition = player.transform.position;
                E.SetActive(true);
            }
            else
            {
                E.SetActive(false);
            }

            if (E.activeSelf && Input.GetKey(KeyCode.E))
            {
                isPlaying = true;
                StartGame();
            }

        }



    }

    public void StartGame() {
        returnPosition = player.transform.position;
        player.transform.position = gamePosition.transform.position;

    }

    public void EndGame() {
        player.transform.position = returnPosition;
        isPlaying = false;
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

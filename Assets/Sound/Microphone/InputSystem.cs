using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    const int DEFAULTMIC = 0;
    const int RECORDING_LENGTH_SECONDS = 10;
    public AudioClip inputClip;
    bool isInit = false;
    public float sensitivity = -1;
    float[] samplesArray = new float[RECORDING_LENGTH_SECONDS*44100];
    public bool isAdjusting = false;
    public float currentPeak = -1;
    public AudioClip sensClip;
    public AudioSource test;

    public GameObject greenBar;
    public GameObject yellowBar;

    public float modifierScale;
    public float modifierPos;
    public float additionPos;


    public void setSensitivity() {
        if (!isAdjusting) {
            sensitivity = 0;
            isAdjusting = true;
            Debug.Log("Recording started");
            sensClip = Microphone.Start(Microphone.devices[DEFAULTMIC], true, 1, 44100);
            StartCoroutine(SetSensitivity());
           
        }
        else isAdjusting = false;
    }

    public void playTestRecordedClip() {
        Microphone.End(Microphone.devices[DEFAULTMIC]);
        RecordedData theClip = new(sensClip, 0);
        FindPeak(theClip);
        test.clip = theClip.internalClip;
        test.timeSamples = theClip.offset;
        float[] testArray = new float[Mathf.RoundToInt(sensClip.length) * 44100 + 1];
        sensClip.GetData(testArray, 0);
        test.Play();
    }

    public IEnumerator SetSensitivity() {
        while (isAdjusting) {
            float[] samples = new float[1];
            if (sensClip != null) {
                sensClip.GetData(samples, Microphone.GetPosition(Microphone.devices[DEFAULTMIC]));
                for (int i = 0; i < samples.Length; i++) {
                    currentPeak = samples[i];
                    currentPeak = Mathf.Clamp01(currentPeak);
                    greenBar.transform.localScale = new Vector2(Mathf.Log10(currentPeak + 0.01f) * modifierScale, greenBar.transform.localScale.y);
                    if (currentPeak > sensitivity) {
                        sensitivity = currentPeak;
                        yellowBar.transform.position = new Vector2(Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(Mathf.Sqrt(currentPeak))))) * modifierPos + additionPos, yellowBar.transform.position.y);
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Recording stopped.");
    }

    public void FindPeak(RecordedData clip) {
        bool noPeak = true;
        int i = 0;
        clip.internalClip.GetData(samplesArray, 0);
        while (noPeak) {
            float vol = Mathf.Sqrt(Mathf.Sqrt(samplesArray[i] * samplesArray[i]));
            if (vol >= sensitivity) {
                Debug.Log("sens " + sensitivity + "\n volume at offset " + vol);
                clip.offset = i;
                Debug.Log("off " + clip.offset);
                noPeak = false;
            }
            i++;
            if (i > 100000) break;
        }
    }

    private void Awake() {
        sensClip = Microphone.Start(Microphone.devices[DEFAULTMIC], false, 1, 44100);
        for (int i = 0; i < Microphone.devices.Length; i++) {
            Debug.Log("Microphone " + i + ": " + Microphone.devices[i]);
        }

        // inputClip = Microphone.Start(Microphone.devices[DEFAULTMIC], true, RECORDING_LENGTH_SECONDS, 44100);
        // isInit = true;
    }

    public void restartRecording() {
        if (!isInit) return;
        if (!Microphone.IsRecording(Microphone.devices[DEFAULTMIC])) return;
        inputClip = Microphone.Start(Microphone.devices[DEFAULTMIC], false, 3, 44100);
    }

    

}

public struct RecordedData
{
    public RecordedData(AudioClip clip, int offset) {
        internalClip = clip;
        this.offset = offset;
    }
    public AudioClip internalClip;
    public int offset;
}
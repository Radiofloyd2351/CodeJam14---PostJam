using Lasp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#region RecordedData structure

/** <summary> Structure representing recorded data 
 * containing an offset to the playback
 * </summary>*/
public struct RecordedData
{
    /**<summary> Creates a <see cref="RecordedData"/> structure  containing 
    * a <paramref name="clip"/> and an <paramref name="offset"/>
    * </summary>
    *<param name="clip"> the audio clip contained in the data </param>
    *<param name="offset"> the offset in samples from the start of the clip
    * where playback starts </param>
    */
    public RecordedData(AudioClip clip, int offset) {
        //audioClip contained in the struct
        internalClip = clip;
        // offset from the beginning of the clip where a peak was found
        this.offset = offset;
    }
    public AudioClip internalClip;
    public int offset;
}

#endregion

#region InputSystem class

/**<summary> System designed to track audio volumes and dynamically 
 * record audio into <see cref="RecordedData"/> clips 
 * </summary>*/
public class InputSystem : MonoBehaviour
{
    private const int DEFAULT_MIC = 0;
    private const int RECORDING_LENGTH_SECONDS = 1;

    #region Attributes

    #region Serialized

    [SerializeField]
    private AudioSource _internalAudioSource;
    [SerializeField]
    private GameObject _sensitivityBar;
    [SerializeField]
    private GameObject _loudestPeakBar;
    [SerializeField]
    private GameObject setBarTotal;
    [SerializeField]
    private GameObject micPointer;
    [SerializeField]
    private SerializedDictionary<string, GameObject> orangeBarDictionary;
    [SerializeField]
    private float modifierScaleOrange;
    [SerializeField]
    private float modifierScale;
    [SerializeField]
    private float modifierPos;
    [SerializeField]
    private float additionPos;
    [SerializeField]
    private TextMeshProUGUI[] mics;

    #endregion

    #region Audio


    private AudioSourceWithOffset _player;
    private AudioLevelTracker _tracker;
    private AudioClip inputClip;
    private List<string> _devices;
    private int currentMic = DEFAULT_MIC;

    #endregion

    #region Necessary Data

    public int maxMicPosition = RECORDING_LENGTH_SECONDS * 44100;
    public float sensitivity = -100f;
    float[] samplesArray = new float[RECORDING_LENGTH_SECONDS * 44100];
    public float currentPeak = -100f;
    public bool isAdjusting = false;

    #endregion

    #endregion

    #region Tracking

    // Creates a tracker
    private void CreateTracker() {
        if (_tracker != null) {
            Destroy(_tracker.gameObject);
            _tracker = null;
        }
        GameObject trackerObj = new GameObject("_tracker");
        trackerObj.transform.parent = gameObject.transform;
        _tracker = trackerObj.AddComponent<AudioLevelTracker>();
    }

    // Sets to the default mic
    private void SetMic() {
        SetMic(currentMic);
    }

    /** <summary> Set the microphone to track in real time </summary>
     * <param name= "i"> index of the microphone to pick from available devices </param>
     * */
    public void SetMic(int i) {
        CreateTracker();
        currentMic = i;
        _tracker.deviceID = _devices[i];
        Debug.Log("did set mic");
    }

    // Sets the sensitivity to the loudest peak it hears
    private IEnumerator CoroutineSetSensitivity() {
        while (isAdjusting) {
            currentPeak = _tracker.inputLevel;
            currentPeak = (currentPeak + 50) / (50) * (100) - 100;
            _sensitivityBar.transform.localScale = new Vector2(1 + (currentPeak * modifierScale), _sensitivityBar.transform.localScale.y);
            if (currentPeak > sensitivity) {
                sensitivity = currentPeak;
                _loudestPeakBar.transform.localPosition = new Vector3((1 + (currentPeak * modifierScale)) * modifierPos + additionPos, _loudestPeakBar.transform.localPosition.y, _loudestPeakBar.transform.localPosition.z);
            }
            yield return null;
        }
    }

    /** <summary> Sets the sensitivity based on the loudest peak
     * of the currently active microphone </summary>*/
    public void SetSensitivity() {
        Debug.Log("Setting Sensitivity");
        if (!isAdjusting) {
            setBarTotal.SetActive(true);
            sensitivity = -100;
            isAdjusting = true;
            StartCoroutine(CoroutineSetSensitivity());
        }
        else {
            isAdjusting = false;
            setBarTotal.SetActive(false);
        }
    }

    // *TODO* Write the logic to track recording progress
    // for each instrument
    private void HandleRecordingBars(string recording) {

    }

    // *TODO* Implement a pointer that will point to the playback
    // position of the clip
    private void  HandlePointer(string recording) {

    }

    #endregion

    #region Recording

    /**<summary> Starts or restarts the recording of incoming microphone audio </summary>
     * <remarks> The recording will be stored in the <see cref="RecordingContainer"/></remarks>
     * <param name="recording"> the name the <see cref="RecordedData"/> will be saved under </param>
     * */
    public void RestartRecording(string recording) {
        if (Microphone.IsRecording(Microphone.devices[currentMic])) {
            Microphone.End(Microphone.devices[currentMic]);
            Debug.Log("Recording done");
            HandlePointer(recording);
            HandleRecordingBars(recording);
            return;
        }
        StartCoroutine(RecordAndStore(recording));
        HandleRecordingBars(recording);
    }

    // Coroutine for recording and storing audio data
    private IEnumerator RecordAndStore(string recording) {
        Debug.Log("recording now");
        inputClip = Microphone.Start(Microphone.devices[currentMic], false, RECORDING_LENGTH_SECONDS, 44100);
        yield return new WaitForSeconds(1);
        AddToRecordings(recording, inputClip);
    }

    // Function that will set the offset of the
    // recorded data clip to the loudest peak found
    private RecordedData LookForLoudestPeak(AudioClip clip) {
        if (clip == null) {
            return new RecordedData(null, 0);
        }
        bool noPeak = true;
        int i = 0;
        bool chosen = false;
        int offset = 0;
        float lastVol = -1000;
        clip.GetData(samplesArray, 0);
        while (noPeak) {
            float vol = 20.0f * Mathf.Log10(samplesArray[i]);
            if (vol >= sensitivity) {
                // sets offset to be the loudest peak
                if (lastVol < vol) {
                    offset = i;
                    Debug.Log("off " + offset);
                    lastVol = vol;
                    Debug.Log("sens " + sensitivity + "\n volume at offset " + vol);
                    chosen = true;
                }
                offset = i;
                chosen = true;
                break;
            }
            i++;

            if (i >= samplesArray.Length) {
                break;
            };
        }
        if (!chosen) {
            return new RecordedData(null, 0);
        }
        if (offset >= 128) { offset -= 128; }
        return new(clip, offset);
    }

    // Function that will find the first offset higher or equal
    // to the sensitivity
    private RecordedData FindPeak(AudioClip clip) {
        if (clip == null) {
            return new RecordedData(null, 0);
        }
        bool noPeak = true;
        int i = 0;
        bool chosen = false;
        int offset = 0;
        clip.GetData(samplesArray, 0);
        while (noPeak) {
            float vol = 20.0f * Mathf.Log10(samplesArray[i]);
            if (vol >= sensitivity) {
                offset = i;
                chosen = true;
                break;
            }
            i++;

            if (i >= samplesArray.Length) {
                break;
            };
        }
        if (!chosen) {
            return new RecordedData(null, 0);
        }
        if (offset >= 128) { offset -= 128; }
        return new(clip, offset);
    }

    // Function that creates and stores the recorded data with an offset
    private void AddToRecordings(string name, AudioClip inputClip) {
        Debug.Log(inputClip);
        RecordedData recording = FindPeak(inputClip);
        if (RecordingContainer.recordings.ContainsKey(name)) {
            RecordingContainer.recordings[name] = recording;
        }
        else {
            RecordingContainer.recordings.Add(name, recording);
        }
    }

    #endregion

    #region Playing

    /**<summary> Play a recording stored in the <see cref="RecordingContainer"/></summary>
     * <param name="recording"> Name of the recording to find in the 
     * <see cref="RecordingContainer"/></param>
     * */
    public void TestRecording(string recording) {
        _player.Play(RecordingContainer.recordings[recording]);
    }

    #endregion

    public void Awake() {
        _player = new(_internalAudioSource);
        _devices = new();
        // Create a devices list
        foreach (DeviceDescriptor device in AudioSystem.InputDevices) {
            _devices.Add(device.ID);
        }
        // Print microphones found
        for (int i = 0; i < Microphone.devices.Length; i++) {
            Debug.Log("Microphone " + i + ": " + Microphone.devices[i]);
            if (i < mics.Length) {
                mics[i].text = Microphone.devices[i];
            }
        }
        SetMic();
    }


    public void Update() {}
}

#endregion


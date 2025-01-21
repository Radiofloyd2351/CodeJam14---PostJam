using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class ZoneDelimiting : MonoBehaviour
{

    [SerializeField]
    private Collider2D _character;
    [SerializeField]
    private ZonePlayer _player;
    private Collider2D _collider;
    [SerializeField]
    private bool started = false;
    [SerializeField]
    public GameObject welcome;
    [SerializeField]
    public float camSize;
    [SerializeField]
    private Level level;
    [SerializeField]
    public Vector2 minBounds;
    [SerializeField]
    public Vector2 maxBounds;


    private bool isDone;

    private bool legit = true;

    private int i = 0;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (legit && !started && collider == _character) {
            StartCoroutine(Timer(collider));
            started = true;
            i++;
        }
        if (!isDone && welcome != null) {
            welcome.SetActive(true);
            StartCoroutine(StartWelcomeAnim(welcome));
        }
        isDone = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(!PlayerStats.transitionning  && legit && started && collision == _character) {
            StartCoroutine(Timer(collision));   
            started = false;
            i++;
        }
    }


    private IEnumerator StartWelcomeAnim(GameObject obj) {
        yield return new WaitForSeconds(3);
        Destroy(obj);
    }




private IEnumerator Timer(Collider2D collider) {
        yield return new WaitForSeconds(0.1f);
        if (i > 1) {
            legit = false;
            yield return Timer2();
        } else {
            i = 0;
            if (legit && started && collider == _character) {
                CameraFollow.currentZone = level;
                _player.SetZoneTrack(level);
            }
        }
    }

    private IEnumerator Timer2() {
        yield return new WaitForSeconds(3f);
        legit = true;
        i = 0;
    }
}

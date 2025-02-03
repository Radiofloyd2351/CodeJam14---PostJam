using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using FMODUnity;

public class ZoneDelimiting : MonoBehaviour {

    [SerializeField]
    private Collider2D _character;
    [SerializeField]
    private GameObject playerObj;
    private PlayerAnims _animator;
    private TopDownCharacterController _controls;
    [SerializeField]
    private float transitionSpeed;
    [SerializeField]
    private SerializedDictionary<Level, int> directions;
    [SerializeField]
    private ZonePlayer _player;
    private Collider2D _collider;
    private static bool started = false;
    [SerializeField]
    public GameObject welcome;
    [SerializeField]
    public float camSize;
    [SerializeField]
    public Level level;
    [SerializeField]
    public Vector2 minBounds;
    [SerializeField]
    public Vector2 maxBounds;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;
        _animator = playerObj.GetComponent<PlayerAnims>();
        _controls = playerObj.GetComponent<TopDownCharacterController>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (CameraFollow.currentZone != level && !started && collider == _character) {
            started = true;
            StartCoroutine(LoopMovement(directions[CameraFollow.currentZone]));
            CameraFollow.currentZone = level;
            _player.SetZoneTrack(level);
            _player.addBlankLayer(level);
        }
        if (welcome != null) {
            StartCoroutine(StartWelcomeAnim(welcome));
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (gameObject.activeInHierarchy && !PlayerStats.transitionning && started && collision == _character) {
            started = false;
            _animator.StopAnim();
            _controls.EnableControls();
        }
    }

    private IEnumerator LoopMovement(int direction) {
        _controls.DisableControls();
        _animator.RunAnim(direction);
        while (started) {
            switch (direction) {
                case 0:
                    playerObj.transform.position += new Vector3(0f, -transitionSpeed, 0f);
                    break;
                case 1:
                    playerObj.transform.position += new Vector3(0f, transitionSpeed, 0f);
                    break;
                case 2:
                    playerObj.transform.position += new Vector3(transitionSpeed, 0f, 0f);
                    break;
                case 3:
                    playerObj.transform.position += new Vector3(-transitionSpeed, 0f, 0f);
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator StartWelcomeAnim(GameObject obj) {
        welcome.SetActive(true);
        yield return new WaitForSeconds(3);
        Destroy(obj);
    }
}

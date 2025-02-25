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
    private SerializedDictionary<Level, Direction> directions;
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
        if (gameObject.activeInHierarchy && !PlayerInfos.transitionning && started && collision == _character) {
            started = false;
        }
    }

    private IEnumerator LoopMovement(Direction direction) {
        _controls.DisableControls();
        _animator.RunAnim(direction);
        Debug.Log("bafanada + " + started + " *ghasp* " + direction);
        int count = 0;
        while (count < 20) {
            switch (direction) {
                case Direction.Down:
                    playerObj.transform.position += new Vector3(0f, -transitionSpeed, 0f);
                    break;
                case Direction.Up:
                    playerObj.transform.position += new Vector3(0f, transitionSpeed, 0f);
                    break;
                case Direction.Right:
                    playerObj.transform.position += new Vector3(transitionSpeed, 0f, 0f);
                    break;
                case Direction.Left:
                    playerObj.transform.position += new Vector3(-transitionSpeed, 0f, 0f);
                    break;
                default:
                    break;
            }

            if (!started) {
                count++;
            }

            yield return new WaitForSeconds(0.01f);
        }
        _animator.StopAnim();
        _controls.EnableControls();
    }

    private IEnumerator StartWelcomeAnim(GameObject obj) {
        welcome.SetActive(true);
        yield return new WaitForSeconds(3);
        Destroy(obj);
    }
}

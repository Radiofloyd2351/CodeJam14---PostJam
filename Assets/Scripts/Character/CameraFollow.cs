using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed = 1.0f;

    private Vector3 offset;
    private Vector3 targetPos;

    static public Level currentZone;

    [SerializeField] private SerializedDictionary<Level, ZoneDelimiting> zones;


    private Camera cam;
    private void Start()
    {
        if (target == null) return;

        offset = transform.position - target.position;

        cam = GetComponent<Camera>();
    }

    private void Update()
    {

        if (target == null) return;

        Vector3 clampedTargetPos = target.position;

        cam.orthographicSize = zones[currentZone].camSize;

        clampedTargetPos.x = Mathf.Clamp(clampedTargetPos.x, zones[currentZone].minBounds.x, zones[currentZone].maxBounds.x);
        clampedTargetPos.y = Mathf.Clamp(clampedTargetPos.y, zones[currentZone].minBounds.y, zones[currentZone].maxBounds.y);


        targetPos = clampedTargetPos + offset;
        targetPos.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}

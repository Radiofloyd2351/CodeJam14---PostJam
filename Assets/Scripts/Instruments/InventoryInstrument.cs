using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInstrument : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public InstrumentInfo info;
    private Vector3 basePos;


    public void OnBeginDrag(PointerEventData eventData) {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        basePos = transform.position;
    }

    public void OnDrag(PointerEventData eventData) {
        if (eventData.pointerCurrentRaycast.isValid) {
            var currentRaycastPosition = eventData.pointerCurrentRaycast.screenPosition;
            transform.position = currentRaycastPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.position = basePos;

    }
}

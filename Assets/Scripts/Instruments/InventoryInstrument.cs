using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryInstrument : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public InstrumentInfo info;
    private Vector3 basePos;
    private bool isEquipped;

    public void OnBeginDrag(PointerEventData eventData) {
        if (!isEquipped) {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        basePos = transform.position;
        Inventory.instance.selectedInstrument = this;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if (!isEquipped && eventData.pointerCurrentRaycast.isValid) {
            var currentRaycastPosition = eventData.pointerCurrentRaycast.screenPosition;
            transform.position = currentRaycastPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.position = basePos;
        Inventory.instance.selectedInstrument = null;
    }

    public void Equip() {
        isEquipped = true;
        GetComponent<UnityEngine.UI.Image>().color = Color.black;
    }

    public void Unequip() {
        isEquipped = false;
        GetComponent<UnityEngine.UI.Image>().color = Color.white;
    }
}

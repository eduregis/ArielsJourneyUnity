using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollableTextWithClick : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private bool isDragging = false;

    public void OnPointerClick(PointerEventData eventData) {
        if (!isDragging) {
            HandleClick();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData) {
    }

    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;
    }

    private void HandleClick() {
        Debug.Log("Objeto clicado: " + gameObject.name);
        UIManager.Instance.CompleteTyping();
    }
}
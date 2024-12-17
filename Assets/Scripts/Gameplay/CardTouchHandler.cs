using UnityEngine;

public class CardTouchHandler : MonoBehaviour {
    private CardFlip cardFlip;
    public int cardIndex;

    private void Awake() {
        cardFlip = GetComponent<CardFlip>();
        if (cardFlip == null) {
            Debug.LogError("CardFlip component not found on the card!");
        }
    }

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.transform == transform) {
                        OnCardTouched();
                    }
                }
            }
        }
    }

    private void OnCardTouched() {
        if (cardFlip != null) {
            GameStateManager.Instance.OnCardSelected(cardFlip, cardIndex);
        }
    }
}

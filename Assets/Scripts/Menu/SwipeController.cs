using UnityEngine;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition; 
    private float swipeThreshold = 50f;

    public delegate void OnSwipeDetected(string direction);
    public static event OnSwipeDetected SwipeDetected;

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                startTouchPosition = touch.position;
            } else if (touch.phase == TouchPhase.Ended) {
                endTouchPosition = touch.position;
                Vector2 swipe = endTouchPosition - startTouchPosition;
                if (swipe.magnitude > swipeThreshold) {
                    if (Mathf.Abs(swipe.x) < Mathf.Abs(swipe.y)) {
                        if (swipe.y > 0) {
                            SwipeDetected?.Invoke("up");
                        } else {
                            SwipeDetected?.Invoke("down");
                        }
                    }
                }
            }
        }
    }
}

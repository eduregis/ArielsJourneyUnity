using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [System.Serializable]
    public class Anchor {
        public string name;
        public GameObject gameObject;
        public string up;
        public string down;
        public string left;
        public string right; 
        public Vector3 position {
            get { return gameObject != null ? gameObject.transform.position : Vector3.zero; }
        }
    }

    public List<Anchor> anchors = new List<Anchor>();
    public float moveSpeed = 5f;
    private Anchor currentAnchor;
    private Vector2 startTouchPosition, endTouchPosition;
    private float swipeThreshold = 100f;
    private void Start() {
        if (anchors.Count > 0) {
            currentAnchor = anchors[0];
            transform.position = currentAnchor.position;
        }
    }

    private void Update() {
        DetectSwipe();
    }

    private void DetectSwipe() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                startTouchPosition = touch.position;
            } else if (touch.phase == TouchPhase.Ended) {
                endTouchPosition = touch.position;
                Vector2 swipe = endTouchPosition - startTouchPosition;
                if (swipe.magnitude > swipeThreshold) {
                    if (Mathf.Abs(swipe.x) < Mathf.Abs(swipe.y)) {
                        if (swipe.y > 0)
                            MoveToAnchorByName(currentAnchor.up);
                        else
                            MoveToAnchorByName(currentAnchor.down);
                    } else {
                        if (swipe.x > 0)
                            MoveToAnchorByName(currentAnchor.right);
                        else
                            MoveToAnchorByName(currentAnchor.left);
                    }
                }
            }
        }
    }

    private void MoveToAnchorByName(string anchorName) {
        if (string.IsNullOrEmpty(anchorName)) return;
        Anchor targetAnchor = anchors.Find(anchor => anchor.name == anchorName);
        
        if (targetAnchor != null) {
            currentAnchor = targetAnchor;
            StopAllCoroutines();
            StartCoroutine(MoveCamera(targetAnchor.position));
        }
    }

    private System.Collections.IEnumerator MoveCamera(Vector3 targetPosition) {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f) {
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }
}

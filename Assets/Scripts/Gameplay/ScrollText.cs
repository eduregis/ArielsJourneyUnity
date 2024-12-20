// ScrollText.cs
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollText : MonoBehaviour {
    public TextMeshProUGUI textDisplay;  
    public float typingSpeed = 0.05f;
    public UIManager uiManager;
    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping = false;

    private void Awake() {
        if (textDisplay != null) {
            textDisplay.raycastTarget = true;
        }
    }

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.transform == transform) {
                        OnPointerClick();
                    }
                }
            }
        }
    }

    
    public void TypeText(string text) {
        fullText = text;
        isTyping = true;

        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeRoutine(text));
    }

    private IEnumerator TypeRoutine(string text) {
        textDisplay.text = "";
        foreach (char letter in text) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        typingCoroutine = null;
        OnTypingComplete();
    }

    public void CompleteTyping() {
        if (isTyping) {
            StopCoroutine(typingCoroutine);
            textDisplay.text = fullText;
            isTyping = false;
            typingCoroutine = null;
            OnTypingComplete();
        } else {
            TapCompleteText();
        }
    }

    private void OnTypingComplete() {
        uiManager.FlipCards();
    }

    private void TapCompleteText() {
        uiManager.TapCompleteText();
    }

    public void OnPointerClick() {
        CompleteTyping();
    }
}

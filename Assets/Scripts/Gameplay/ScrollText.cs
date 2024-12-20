using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScrollText : MonoBehaviour {
    public TextMeshProUGUI textDisplay;  
    public float typingSpeed = 0.05f;
    public UIManager uiManager;
    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping = false;

    private Dictionary<string, string> customTags = new Dictionary<string, string> {
        { "orange", "#FFA500" },
        { "red", "#FF0000" },
        { "blue", "#0000FF" }
    };

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
        // Processar as tags personalizadas antes de começar a digitação
        fullText = ReplaceCustomTags(text);
        isTyping = true;
        if (typingCoroutine != null) {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeRoutine(fullText));
    }

    private IEnumerator TypeRoutine(string text) {
        textDisplay.text = "";
        int charIndex = 0;
        bool insideTag = false;

        while (charIndex < text.Length) {
            // Detectar início e fim de tags
            if (text[charIndex] == '<') insideTag = true;
            if (text[charIndex] == '>') insideTag = false;

            // Apenas exibir o texto visível, incluindo tags completas
            textDisplay.text += text[charIndex];
            charIndex++;

            // Pular atraso enquanto dentro de uma tag
            if (!insideTag) {
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        isTyping = false;
        typingCoroutine = null;
        OnTypingComplete();
    }

    public void CompleteTyping() {
        if (isTyping) {
            StopCoroutine(typingCoroutine);
            textDisplay.text = fullText; // Exibir o texto completo com formatação
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

    // Função para processar tags personalizadas no texto
    private string ReplaceCustomTags(string text) {
        string processedText = text;

        foreach (var tag in customTags) {
            // Substituir abertura da tag personalizada
            string openingTag = $"<{tag.Key}>";
            string richOpeningTag = $"<color={tag.Value}>";
            processedText = processedText.Replace(openingTag, richOpeningTag);

            // Substituir fechamento da tag personalizada
            string closingTag = $"</{tag.Key}>";
            string richClosingTag = "</color>";
            processedText = processedText.Replace(closingTag, richClosingTag);
        }

        return processedText;
    }
}

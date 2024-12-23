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
        { "blue", "#0000FF" },
        { "green", "#006405" }
    };

    private void Awake() {
        if (textDisplay != null) {
            textDisplay.raycastTarget = true;
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
    float lastSoundTime = 0f;  // Para controlar o intervalo entre os sons

    while (charIndex < text.Length) {
        // Detectar início e fim de tags
        if (text[charIndex] == '<') insideTag = true;
        if (text[charIndex] == '>') insideTag = false;

        // Apenas exibir o texto visível, incluindo tags completas
        textDisplay.text += text[charIndex];
        charIndex++;

        if (text[charIndex - 1] == '\n') {
            yield return new WaitForSeconds(0.25f);
            AudioManager.Instance.PlayOneShot("Effect_Katching");
            yield return new WaitForSeconds(0.25f);
        }

        if (!insideTag) {
            if (Time.time - lastSoundTime >= typingSpeed) {
                AudioManager.Instance.PlayRandomTypingSound();
                lastSoundTime = Time.time;
            }
            yield return new WaitForSeconds(typingSpeed);  // Aguarda o tempo de digitação para o próximo caractere
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
        AudioManager.Instance.PlayOneShot("Effect_Katching");
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

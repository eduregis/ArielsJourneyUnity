using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScrollText : MonoBehaviour {
    public TextMeshProUGUI textDisplay;  
    public float typingSpeed = 0.05f;
    public UIManager uiManager;
    public RectTransform scrollTop;      // Referência à parte superior do pergaminho
    public RectTransform scrollBody;    // Referência ao corpo do pergaminho
    public RectTransform scrollBottom;  // Referência à parte inferior do pergaminho
    public float animationDuration = 0.5f; // Duração da animação de abrir/fechar

    private Vector3 topStartPos;
    private Vector3 bottomStartPos;

    private const float topExpandedY = 210f;    // Posição Y final do topo ao abrir
    private const float bottomExpandedY = -210f; // Posição Y final do bottom ao abrir
    private const float bodyExpandedHeight = 650f; // Altura final do corpo ao abrir
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
        topStartPos = scrollTop.anchoredPosition;
        bottomStartPos = scrollBottom.anchoredPosition;
    }

    public void TypeText(string text) {
        OpenScroll(() => {
            // Processar as tags personalizadas antes de começar a digitação
            fullText = ReplaceCustomTags(text);
            isTyping = true;
            if (typingCoroutine != null) {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeRoutine(fullText));
        });
    }

    public void OpenScroll(System.Action onComplete = null) {
        StartCoroutine(AnimateScroll(true, onComplete));
    }

    public void CloseScroll(System.Action onComplete = null) {
        StartCoroutine(AnimateScroll(false, onComplete));
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

    private IEnumerator AnimateScroll(bool isOpening, System.Action onComplete) {
        float elapsed = 0f;

        // Define os alvos baseados na direção (abrindo ou fechando)
        Vector3 topTargetPos = isOpening ? new Vector3(topStartPos.x, topExpandedY, topStartPos.z) : topStartPos;
        Vector3 bottomTargetPos = isOpening ? new Vector3(bottomStartPos.x, bottomExpandedY, bottomStartPos.z) : bottomStartPos;
        float bodyTargetHeight = isOpening ? bodyExpandedHeight : 0f;

        while (elapsed < animationDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);

            // Lerp para as posições do topo e do bottom
            scrollTop.localPosition = Vector3.Lerp(scrollTop.localPosition, topTargetPos, t);
            scrollBottom.localPosition = Vector3.Lerp(scrollBottom.localPosition, bottomTargetPos, t);

            // Ajusta a altura do corpo
            Vector2 bodySize = scrollBody.sizeDelta;
            bodySize.y = Mathf.Lerp(scrollBody.sizeDelta.y, bodyTargetHeight, t);
            scrollBody.sizeDelta = bodySize;

            yield return null;
        }

        // Garante os valores finais
        scrollTop.localPosition = topTargetPos;
        scrollBottom.localPosition = bottomTargetPos;
        scrollBody.sizeDelta = new Vector2(scrollBody.sizeDelta.x, bodyTargetHeight);

        onComplete?.Invoke();
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

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    [Header("References")]
    public ConfigSheetController configSheetController;
    public GameObject tableContainer;
    public TextMeshProUGUI descriptionText;
    public GameObject firstCard;
    public GameObject secondCard;
    public CardFlip firstCardFlip;
    public CardFlip secondCardFlip;
    public ScrollText scrollText;
    public GameObject leftMultipleChoicesContainer, rightMultipleChoicesContainer;
    public List<Button> buttons;

    private bool hasntNullValues;
    private Vector3 leftMCHiddenPosition = new (-250, 0, 0); // Posição escondida à esquerda
    private Vector3 rightMCHiddenPosition = new(250, 0, 0); // Posição escondida à direita
    private Vector3 leftMCVisiblePosition = new (250, 0, 0); // Posição visível à esquerda
    private Vector3 rightMCVisiblePosition = new (-250, 0, 0);
    private Dialogue actualDialogue; 

    private void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GameplayAnchorManager.Instance.ShowContainer(false);
        FlipCards();
        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Top, 2f, () => {
            GameStateManager.Instance.StartGame();
        });
    }

    public void UpdateUI(Dialogue dialogue) {
        GameplayAnchorManager.Instance.ShowContainer(true);
        hasntNullValues = 
            dialogue.firstCardText != "" &&  dialogue.firstCardImage != "" && 
            dialogue.secondCardText != "" &&  dialogue.secondCardImage != "";
        firstCard.SetActive(hasntNullValues);
        secondCard.SetActive(hasntNullValues);

        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Middle, 0.5f, () => {
            actualDialogue = dialogue;
            StartWritingText(dialogue.descriptionText);
            if (hasntNullValues) {
                PrepareCards(dialogue.firstCardText, dialogue.firstCardImage, dialogue.secondCardText, dialogue.secondCardImage);
            };
        });
    }

    private void GetMultipleChoices(Dialogue dialogue) {
        StartCoroutine(AnimateMovement(leftMultipleChoicesContainer.GetComponent<RectTransform>(), leftMCVisiblePosition, 0.5f, null));
        StartCoroutine(AnimateMovement(rightMultipleChoicesContainer.GetComponent<RectTransform>(), rightMCVisiblePosition, 0.5f,  null));
        ShowButtons(dialogue.multipleChoices);
    }

    private IEnumerator AnimateMovement(RectTransform container, Vector2 targetPosition, float transitionTime, System.Action onComplete) {
        float timeElapsed = 0;
        Vector2 startPosition = container.anchoredPosition; 
        while (timeElapsed < transitionTime) {
            container.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        container.anchoredPosition = targetPosition;
        onComplete?.Invoke();
    }

    private void ShowButtons(MultipleChoice[] multipleChoices) {
        // Desativa todos os botões antes de reativá-los
        foreach (var button in buttons) {
            button.gameObject.SetActive(false);
        }

        // Ativa e define texto nos botões necessários
        for (int i = 0; i < multipleChoices.Length && i < buttons.Count; i++) {
            buttons[i].gameObject.SetActive(true);
            var buttonText = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) {
                buttonText.text = multipleChoices[i].title;
            } else {
                Debug.LogError($"TextMeshProUGUI não encontrado no botão {i}");
            }
        }
    }

    private void StartWritingText(string text) {
        if (scrollText != null) {
            scrollText.TypeText(text);
        } else {
            Debug.LogError("ScrollText is not assigned in the UIManager!");
        }
    }

    public void HideMultipleChoicesContainers() {
        StartCoroutine(AnimateMovement(leftMultipleChoicesContainer.GetComponent<RectTransform>(), leftMCHiddenPosition, 0.5f, () => {
            foreach (var button in buttons) {
            button.gameObject.SetActive(false);
        }
        }));
        StartCoroutine(AnimateMovement(rightMultipleChoicesContainer.GetComponent<RectTransform>(), rightMCHiddenPosition, 0.5f, null));
    }

    public void CompleteTyping() {
        if (scrollText != null) {
            scrollText.CompleteTyping();
        } else {
            Debug.LogError("ScrollText is not assigned in the UIManager!");
        }
    }

    public void ChangeBackground(string backgroundName) {
        BackgroundManager.Instance.ChangeBackground(backgroundName);
    }

    public void ClearText() {
        descriptionText.text = "";
    }

    private void PrepareCards(string firstCardText, string firstCardImage, string secondCardText, string secondCardImage) {
        firstCardFlip.SetCardText(firstCardText);
        firstCardFlip.SetCardImage(firstCardImage);
        secondCardFlip.SetCardText(secondCardText);
        secondCardFlip.SetCardImage(secondCardImage);
    }

    public void FlipCards() {
        if (actualDialogue != null) {
            bool isMultipleChoices = actualDialogue.multipleChoices.Length > 0;
            if (isMultipleChoices) {
            GetMultipleChoices(actualDialogue);
            } 
            GameDataManager.SetMultipleChoices(isMultipleChoices);
        }
        firstCardFlip.FlipCard();
        secondCardFlip.FlipCard();
    }

    public void TapCompleteText() {
        if (!hasntNullValues) {
            GameStateManager.Instance.OnLetterTapped();
        }
    }

    // Button Actions
    public void OnConfigButtonPressed() {
        if (configSheetController != null) {
            configSheetController.ShowSheet();
        } else {
            Debug.LogError("ConfigSheetController não foi atribuído ao UIManager!");
        }
        Debug.Log("Config Button Pressed");
    }

    public void OnBackButtonPressed() {
        // Sua ação para o BackButton
        Debug.Log("Back Button Pressed");
    }

    public void OnMultipleChoiceSelected(int index) {
        int nextDialogueId = actualDialogue.multipleChoices[index].nextDialogueId;
        if (nextDialogueId != 0) {
            GameStateManager.Instance.OnMultipleChoicesSelected(nextDialogueId);
        } else {
            string description = actualDialogue.multipleChoices[index].description;
            StartWritingText(description);
        }
        
    }
}

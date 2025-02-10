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

        //GetMultipleChoices(dialogue);

        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Middle, 0.5f, () => {
            StartWritingText(dialogue.descriptionText);
            if (hasntNullValues) {
                PrepareCards(dialogue.firstCardText, dialogue.firstCardImage, dialogue.secondCardText, dialogue.secondCardImage);
            };
        });
    }

    private void GetMultipleChoices(Dialogue dialogue) {
        bool isMultipleChoices = dialogue.multipleChoices.Length > 0;
        if (isMultipleChoices) {
            StartCoroutine(AnimateMovement(leftMultipleChoicesContainer, leftMCVisiblePosition, 1f, () => {
                Debug.Log("Animação do contêiner esquerdo concluída!");
            }));

            StartCoroutine(AnimateMovement(rightMultipleChoicesContainer, rightMCVisiblePosition, 1f, () => {
                Debug.Log("Animação do contêiner direito concluída!");
            }));
            ShowButtons(dialogue.multipleChoices);
        }
        GameDataManager.SetMultipleChoices(isMultipleChoices);
    }

    private IEnumerator AnimateMovement(GameObject container, Vector3 targetPosition, float transitionTime, System.Action onComplete) {
        float timeElapsed = 0;
        Vector3 startPosition = container.transform.localPosition; // Usamos localPosition para trabalhar com coordenadas locais
        while (timeElapsed < transitionTime) {
            container.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        container.transform.localPosition = targetPosition;
        onComplete?.Invoke();
    }

    private void ShowButtons(MultipleChoice[] multipleChoices) {
        foreach (var button in buttons) {
            button.gameObject.SetActive(false);
        }
        for (int i = 0; i < multipleChoices.Length && i < buttons.Count; i++) {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = multipleChoices[i].title;
        }
    }

    private void StartWritingText(string text) {
        if (scrollText != null) {
            scrollText.TypeText(text);
        } else {
            Debug.LogError("ScrollText is not assigned in the UIManager!");
        }
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
}

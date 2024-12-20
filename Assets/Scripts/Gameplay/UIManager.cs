using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
    public GameObject tableContainer;
    public TextMeshProUGUI descriptionText;
    public GameObject firstCard;
    public GameObject secondCard;
    public CardFlip firstCardFlip;
    public CardFlip secondCardFlip;
    public ScrollText scrollText;

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
        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Middle, 0.5f, () => {
            StartWritingText(dialogue.descriptionText);
            PrepareCards(dialogue.firstCardText, dialogue.firstCardImage, dialogue.secondCardText, dialogue.secondCardImage);
        });
    }

    private void StartWritingText(string text) {
        // Começa a digitação do texto no pergaminho
        if (scrollText != null) {
            scrollText.TypeText(text);
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

    public void SetButtonListeners(UnityEngine.Events.UnityAction firstAction, UnityEngine.Events.UnityAction secondAction) {
        // Defina os listeners dos botões de escolha
    }
}

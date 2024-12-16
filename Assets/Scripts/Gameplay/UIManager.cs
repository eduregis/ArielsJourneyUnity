using UnityEngine;
using TMPro;
using System.Collections;

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
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateUI(Dialogue dialogue) {
        StartCoroutine(SetupNewDialogue(dialogue));
    }

    IEnumerator SetupNewDialogue(Dialogue dialogue) {
        FlipCards();
        yield return new WaitForSeconds(2.0f);
        tableContainer.SetActive(true);
        StartWritingText(dialogue.descriptionText);
        PrepareCards(dialogue.firstCardText, dialogue.secondCardText);
    }

    private void StartWritingText(string text) {
        // Começa a digitação do texto no pergaminho
        if (scrollText != null) {
            scrollText.TypeText(text);
        } else {
            Debug.LogError("ScrollText is not assigned in the UIManager!");
        }
    }

    private void PrepareCards(string firstCardText, string secondCardText) {
        firstCardFlip.SetCardText(firstCardText);
        secondCardFlip.SetCardText(secondCardText);
    }

    public void FlipCards() {
        // Quando o texto terminar de ser escrito, viramos as cartas
        firstCardFlip.FlipCard();
        secondCardFlip.FlipCard();
    }


    public void SetButtonListeners(UnityEngine.Events.UnityAction firstAction, UnityEngine.Events.UnityAction secondAction) {
        // Defina os listeners dos botões de escolha
    }
}

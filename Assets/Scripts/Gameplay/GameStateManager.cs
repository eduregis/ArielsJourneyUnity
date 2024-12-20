// GameStateManager.cs
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance;

    private StageData currentStage;
    private Dialogue currentDialogue;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Inicia o game com Stage 0 
    public void StartGame() {
        LoadStage(0);
    }

    private void LoadStage(int stageId) {
        currentStage = Resources.Load<StageData>($"Stages/Stage_{stageId}");
        if (currentStage == null) {
            Debug.LogError($"Stage {stageId} not found in Resources!");
            return;
        }
        UIManager.Instance.ChangeBackground(currentStage.backgroundName);
        LoadDialogue(0);
    }

    private void LoadDialogue(int dialogueId) {
        currentDialogue = currentStage.GetDialogueById(dialogueId);
        if (currentDialogue == null) {
            Debug.LogError($"Dialogue {dialogueId} not found in Stage {currentStage.stageId}!");
            return;
        }
        Debug.Log($"Loaded dialogue: {currentDialogue.firstCardText}, {currentDialogue.secondCardText}");
        UpdateUI();
    }

    private void UpdateUI() {
        UIManager.Instance.UpdateUI(currentDialogue);
    }

    public void OnCardSelected(CardFlip selectedCard, int cardIndex) {
        if (selectedCard == null || selectedCard.isFlipping) {
            Debug.LogWarning("Card is already flipping or null!");
            return;
        }

        // Anima a seleção da carta
        selectedCard.AnimateSelection(0.5f, () => {
            Debug.Log($"Card selected: {selectedCard.frontCardText.text}");
            switch (cardIndex) {
                case 1:
                    OnChoiceSelected(currentDialogue.nextFirstDialogueId);
                    break;
                case 2:
                    OnChoiceSelected(currentDialogue.nextSecondDialogueId);
                    break;
                default:
                    break;
            }
        });
    }

    private void OnChoiceSelected(int nextDialogueId) {
        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Bottom, 0.5f, () => {
            GameplayAnchorManager.Instance.ShowContainer(false);
            UIManager.Instance.FlipCards();
            UIManager.Instance.ClearText();
            GoToNextDialogue(nextDialogueId);
        });
    }

    private void GoToNextDialogue(int nextDialogueId) {
        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Top, 0.5f, () => {
            if (nextDialogueId == -1) { // Supondo que -1 significa fim do Stage
                if (currentStage.nextStageId != -1) {
                    LoadStage(currentStage.nextStageId);
                } else {
                    Debug.Log("Game completed!");
                }
            } else {
                LoadDialogue(nextDialogueId);
            }
        });
    }
}

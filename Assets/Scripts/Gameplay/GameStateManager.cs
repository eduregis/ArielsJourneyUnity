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

    private void Start() {
        LoadStage(0); // Inicializa com o Stage 0
    }

    private void LoadStage(int stageId) {
        currentStage = Resources.Load<StageData>($"Stages/Stage_{stageId}");
        if (currentStage == null) {
            Debug.LogError($"Stage {stageId} not found in Resources!");
            return;
        }
        LoadDialogue(0); // Começa pelo primeiro diálogo do estágio
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

    private void OnChoiceSelected(int nextDialogueId) {
        if (nextDialogueId == -1) { // Supondo que -1 significa fim do Stage
            if (currentStage.nextStageId != -1) {
                LoadStage(currentStage.nextStageId);
            } else {
                Debug.Log("Game completed!");
            }
        } else {
            LoadDialogue(nextDialogueId);
        }
    }
}

// GameStateManager.cs
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance;
    public ScrollText scrollText;
    private StageData currentStage;
    private Dialogue currentDialogue;


    private const string HerosJourneyKey = "HerosJourney";
    private const string ArchetypesKey = "Archetypes";

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        GameDataManager.Initialize();
    }

    // Inicia o game com Stage 0 
    public void StartGame() {
        int stage = GameDataManager.GetStage();
        LoadStage(stage);
    }

    private void LoadStage(int stageId) {
        currentStage = Resources.Load<StageData>($"Stages/Stage_{stageId}");
        if (currentStage == null) {
            Debug.LogError($"Stage {stageId} not found in Resources!");
            return;
        }
        UIManager.Instance.ChangeBackground(currentStage.backgroundName);
        Debug.Log(currentStage.sound);
        if (currentStage.sound != "") {
            AudioManager.Instance.Play(currentStage.sound);
        }
        int dialogue = GameDataManager.GetDialogue();
        LoadDialogue(dialogue);
    }

    private void LoadDialogue(int dialogueId) {
        currentDialogue = currentStage.GetDialogueById(dialogueId);
        if (currentDialogue == null) {
            Debug.LogError($"Dialogue {dialogueId} not found in Stage {currentStage.stageId}!");
            return;
        }
        if (currentDialogue.sound != "") {
            AudioManager.Instance.PlayOneShot(currentDialogue.sound);
        }
        UpdateUI();
        GetTriggers();
    }

    private void UpdateUI() {
        UIManager.Instance.UpdateUI(currentDialogue);
    }

    private void GetTriggers() {
        foreach (string trigger in currentDialogue.triggers) {
            if (trigger.StartsWith("HJ_")) {
                ProcessHerosJourney(trigger);
            } else if (trigger.StartsWith("AT_")) {
                ProcessArchetype(trigger);
            }
        };
    }

    private void ProcessHerosJourney(string flag) {
        GameDataManager.ProcessHerosJourney(flag);
    }

    private void ProcessArchetype(string flag) {
        GameDataManager.AddArchetype(flag);
    }

    public void OnCardSelected(CardFlip selectedCard, int cardIndex) {
        if (selectedCard == null || selectedCard.isFlipping) {
            Debug.LogWarning("Card is already flipping or null!");
            return;
        }

        // Anima a seleção da carta
        AudioManager.Instance.PlayOneShot("Effect_Card_Side_Choosen");
        selectedCard.AnimateSelection(0.5f, () => {
            Debug.Log($"Card selected: {cardIndex}");
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

    public void OnLetterTapped() {
        OnChoiceSelected(currentDialogue.nextFirstDialogueId);
    }

    private void OnChoiceSelected(int nextDialogueId) {
        scrollText.CloseScroll(() => {
            GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Bottom, 0.5f, () => {
                GameplayAnchorManager.Instance.ShowContainer(false);
                UIManager.Instance.FlipCards();
                UIManager.Instance.ClearText();
                GoToNextDialogue(nextDialogueId);
            });
        });
    }

    private void GoToNextDialogue(int nextDialogueId) {
        GameplayAnchorManager.Instance.MoveContainerToAnchor(GameplayAnchorType.Top, 0.5f, () => {
            if (nextDialogueId == -1) { // Supondo que -1 significa fim do Stage
                if (currentStage.nextStageId != -1) {
                    GameDataManager.SetStage(currentStage.nextStageId);
                    GameDataManager.SetDialogue(0);
                    LoadStage(currentStage.nextStageId);
                } else {
                    Debug.Log("Game completed!");
                }
            } else {
                GameDataManager.SetDialogue(nextDialogueId);
                LoadDialogue(nextDialogueId);
            }
        });
    }
}

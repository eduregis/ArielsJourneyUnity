using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StageData", menuName = "Game/StageData")]
public class StageData : ScriptableObject {
    public int stageId;
    public int nextStageId; // ID do próximo estágio ou -1 para indicar fim do jogo;
    public List<Dialogue> dialogues;
    public string backgroundName;
    public string sound;

    public Dialogue GetDialogueById(int id) {
        return dialogues.Find(dialogue => dialogue.dialogueId == id);
    }
}
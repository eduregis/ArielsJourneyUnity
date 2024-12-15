using UnityEngine;

[System.Serializable]
public class Dialogue {
    public int dialogueId;
    public string descriptionText;
    public string firstCardText;
    public string secondCardText;
    public Sprite firstCardImage;
    public Sprite secondCardImage;
    public int nextFirstDialogueId; // -1 para indicar fim do Stage
    public int nextSecondDialogueId; // -1 para indicar fim do Stage
}

using UnityEngine;

[System.Serializable]
public class Dialogue {
    public int dialogueId;
    [TextArea(3,10)]
    public string descriptionText;
    public string firstCardText;
    public string secondCardText;
    public string firstCardImage;
    public string secondCardImage;
    public int nextFirstDialogueId; // -1 para indicar fim do Stage
    public int nextSecondDialogueId; // -1 para indicar fim do Stage
    public string[] triggers;
    public string sound;
    public MultipleChoice[] multipleChoices;
}

[System.Serializable]
public class MultipleChoice {
    public int multipleChoiceId;
    public string title;
    
    [TextArea(3,10)]
    public string description;
    public int nextDialogueId;
}
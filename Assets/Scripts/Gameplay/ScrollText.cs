// ScrollText.cs
using TMPro;
using UnityEngine;
using System.Collections;

public class ScrollText : MonoBehaviour {
    public TextMeshProUGUI textDisplay;  
    public float typingSpeed = 0.05f;

    // Referência para o UIManager, ou pode ser referência ao GameStateManager
    public UIManager uiManager;

    public void TypeText(string text) {
        StartCoroutine(TypeRoutine(text));
    }

    private IEnumerator TypeRoutine(string text) {
        textDisplay.text = "";
        foreach (char letter in text) {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        
        // Chama o método de flipar as cartas após terminar a digitação
        uiManager.FlipCards();
    }
}

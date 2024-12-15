using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;

    [Header("Dialogue UI")]
    public TextMeshProUGUI descriptionText; // Alterado para TextMeshProUGUI
    public Button firstButton;
    public Button secondButton;
    public Image firstCardImage;
    public Image secondCardImage;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Atualiza o texto e imagens da UI para o diálogo atual.
    /// </summary>
    /// <param name="description">Texto do diálogo.</param>
    /// <param name="firstButtonText">Texto do primeiro botão.</param>
    /// <param name="secondButtonText">Texto do segundo botão.</param>
    /// <param name="firstImage">Sprite para a imagem do primeiro botão (opcional).</param>
    /// <param name="secondImage">Sprite para a imagem do segundo botão (opcional).</param>
    public void UpdateDialogueUI(
        string description,
        string firstCardText,
        string secondCardText,
        Sprite firstImage = null,
        Sprite secondImage = null
    ) {
        // Atualiza os textos
        descriptionText.text = description;
        Debug.Log(description + " , " + firstCardText + " , " + secondCardText + " , " + firstImage  + " , " + secondImage);
        firstButton.GetComponentInChildren<TextMeshProUGUI>().text = firstCardText;
        secondButton.GetComponentInChildren<TextMeshProUGUI>().text = secondCardText;

        // Atualiza as imagens
        UpdateButtonImage(firstCardImage, firstImage);
        UpdateButtonImage(secondCardImage, secondImage);
    }

    /// <summary>
    /// Adiciona listeners para os botões de escolha.
    /// </summary>
    /// <param name="onFirstChoice">Ação para o primeiro botão.</param>
    /// <param name="onSecondChoice">Ação para o segundo botão.</param>
    public void SetButtonListeners(UnityEngine.Events.UnityAction onFirstChoice, UnityEngine.Events.UnityAction onSecondChoice) {
        firstButton.onClick.RemoveAllListeners();
        secondButton.onClick.RemoveAllListeners();

        firstButton.onClick.AddListener(onFirstChoice);
        secondButton.onClick.AddListener(onSecondChoice);
    }

    /// <summary>
    /// Atualiza a imagem do botão ou oculta se a imagem for nula.
    /// </summary>
    private void UpdateButtonImage(Image buttonImage, Sprite sprite) {
        if (sprite != null) {
            buttonImage.sprite = sprite;
            buttonImage.gameObject.SetActive(true);
        } else {
            buttonImage.gameObject.SetActive(false);
        }
    }
}

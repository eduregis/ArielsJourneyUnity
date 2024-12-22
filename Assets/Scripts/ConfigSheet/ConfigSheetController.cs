using UnityEngine;

public class ConfigSheetController : MonoBehaviour {
    [Header("References")]
    public GameObject  configSheetContainer; // Referência ao Canvas que contém o painel ConfigSheet
    public GameObject sheet;        // Painel do ConfigSheet
    public GameObject overlay;      // Área de fundo clicável para fechar o ConfigSheet

    [Header("Animation")]
    private Animator animator;      // Controlador de animação do ConfigSheet
    private static readonly string ShowTrigger = "Show"; // Nome do trigger para exibir o ConfigSheet
    private static readonly string HideTrigger = "Hide"; // Nome do trigger para ocultar o ConfigSheet

    void Start() {
        // Certifique-se de que o Canvas está desativado no início
        if (configSheetContainer != null) {
            configSheetContainer.SetActive(false);
        }

        // Obter o Animator do ConfigSheet
        if (sheet != null) {
            animator = configSheetContainer.GetComponent<Animator>();
        } else {
            Debug.LogError("O GameObject 'sheet' não foi atribuído no Inspector!");
        }
    }

    public void ShowSheet() {
        if (configSheetContainer != null) {
            configSheetContainer.SetActive(true);
        }

        if (animator != null) {
            animator.SetTrigger(ShowTrigger); // Disparar animação de entrada
        } else {
            Debug.LogError("Animator não está configurado no 'sheet'!");
        }
    }

    public void HideSheet() {
        Debug.Log("escondeu");
        if (animator != null) {
            animator.SetTrigger(HideTrigger); // Disparar animação de saída
        } else {
            Debug.LogError("Animator não está configurado no 'sheet'!");
        }

        // Desativar o Canvas após um pequeno atraso para permitir a animação
        StartCoroutine(DisableCanvasAfterAnimation());
    }

    private System.Collections.IEnumerator DisableCanvasAfterAnimation() {
        // Espera o tempo de duração da animação
        yield return new WaitForSeconds(0.5f); // Ajuste de acordo com a duração da animação

        if (configSheetContainer != null) {
            configSheetContainer.SetActive(false); // Desativar o Canvas
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour {
    public enum ButtonType { Config, Back }
    public ButtonType buttonType;

   public Image buttonImage; // Referência para o componente Image
    public float touchedAlpha = 0.5f; // O valor do alpha durante o toque
    private float originalAlpha; // Armazena o valor original do alpha

    private void Start() {
         buttonImage = GetComponent<Image>();
        if (buttonImage != null) {
            originalAlpha = buttonImage.color.a;
        }
    }

    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.transform == transform) {
                    if (touch.phase == TouchPhase.Began) {
                        OnTouchStarted();
                    }

                    if (touch.phase == TouchPhase.Ended) {
                        OnTouchEnded();
                    }
                }
            }
        }
    }

    private void OnTouchStarted() {
        // Altera o alpha da imagem para o valor durante o toque
        if (buttonImage != null) {
            Color color = buttonImage.color;
            color.a = touchedAlpha;
            buttonImage.color = color;
        }
    }

    private void OnTouchEnded() {
        // Restaura o alpha original
        if (buttonImage != null) {
            Color color = buttonImage.color;
            color.a = originalAlpha;
            buttonImage.color = color;
        }

        // Chama a função de toque apenas quando o toque terminar
        if (buttonType == ButtonType.Config) {
            UIManager.Instance.OnConfigButtonPressed();
        } else if (buttonType == ButtonType.Back) {
            UIManager.Instance.OnBackButtonPressed();
        }
    }
}
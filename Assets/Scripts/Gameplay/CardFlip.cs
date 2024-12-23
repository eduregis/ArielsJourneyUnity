using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour {
    public GameObject frontCard;
    public GameObject backCard;
    public TextMeshProUGUI frontCardText;
    public Image cardImage;
    public float flipDuration = 0.5f;
    public  bool isFlipped = false;
    public bool isFlipping = false;
    public bool isSelected = false;
    public int cardIndex = 0;    

    private void Update() {
        if (isFlipped) {
            float rotation = Mathf.LerpAngle(frontCard.transform.rotation.eulerAngles.y, 180f, Time.deltaTime / flipDuration);
            frontCard.transform.rotation = Quaternion.Euler(0, rotation, 0);
            backCard.transform.rotation = Quaternion.Euler(0, rotation - 180f, 0);

            if (rotation >= 90f && isFlipping) {
                frontCard.SetActive(false);
                backCard.SetActive(true);
                isFlipping = false;
            }
        } else {
            float rotation = Mathf.LerpAngle(frontCard.transform.rotation.eulerAngles.y, 0f, Time.deltaTime / flipDuration);
            frontCard.transform.rotation = Quaternion.Euler(0, rotation, 0);
            backCard.transform.rotation = Quaternion.Euler(0, rotation - 180f, 0);

            if (rotation <= 90f && isFlipping) {
                backCard.SetActive(false);
                frontCard.SetActive(true);
                isFlipping = false;
            }
        }
    }

    public void FlipCard() {
        isFlipped = !isFlipped;
        isFlipping = true;
    }

    public void SetCardText(string frontText) {
        if (frontCardText != null) {
            frontCardText.text = frontText;
            isSelected = false;
        } else {
            Debug.LogError("TextMeshProUGUI component is missing on the front card!");
        }
    }

    public void SetCardImage(string imageName) {
       if (string.IsNullOrEmpty(imageName)) {
            Debug.LogError("Image name is null or empty!");
            return;
        }

        string imagePathPrefix = "Images/Cards/";
        string fullPath = imagePathPrefix + imageName;

        Sprite loadedSprite = Resources.Load<Sprite>(fullPath);

        if (loadedSprite != null) {
            cardImage.sprite = loadedSprite;
            Debug.Log($"Image '{imageName}' successfully loaded and applied.");
        } else {
            Debug.LogError($"Image '{imageName}' not found in Resources folder!");
        }
    }

    public void AnimateSelection(float duration, System.Action onComplete = null) {
        if (!isSelected && !isFlipped) {
            isSelected = true;
            StartCoroutine(SelectionAnimation(duration, onComplete));
        }
    }

    private IEnumerator SelectionAnimation(float duration, System.Action onComplete) {
        Vector3 originalScale = transform.localScale;
        Vector3 largerScale = originalScale * 1.2f; // Define o tamanho ampliado da carta
        float halfDuration = duration / 2;

        // Crescer
        float elapsedTime = 0f;
        while (elapsedTime < halfDuration) {
            transform.localScale = Vector3.Lerp(originalScale, largerScale, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = largerScale;

        // Diminuir
        elapsedTime = 0f;
        while (elapsedTime < halfDuration) {
            transform.localScale = Vector3.Lerp(largerScale, originalScale, elapsedTime / halfDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originalScale;

        // Callback
        onComplete?.Invoke();
    }

    public void OnCardSelected() {
        GameStateManager.Instance.OnCardSelected(this, cardIndex);
    }
}

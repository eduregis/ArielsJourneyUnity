using UnityEngine;
using TMPro;

public class CardFlip : MonoBehaviour {
    public GameObject frontCard;
    public GameObject backCard;
    public TextMeshProUGUI frontCardText;
    public float flipDuration = 0.5f;
    private bool isFlipped = false;
    private bool isFlipping = false;

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
        } else {
            Debug.LogError("TextMeshProUGUI component is missing on the front card!");
        }
    }
}

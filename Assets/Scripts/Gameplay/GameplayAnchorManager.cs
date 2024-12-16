using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum GameplayAnchorType
{
    Top,
    Middle,
    Bottom
}

public class GameplayAnchorManager : MonoBehaviour {
    public GameObject anchorTop;    // Referência ao GameObject da âncora superior
    public GameObject anchorMiddle; // Referência ao GameObject da âncora do meio
    public GameObject anchorBottom; // Referência ao GameObject da âncora inferior

    public GameObject container; // O container a ser movido

    private Dictionary<GameplayAnchorType, GameObject> anchors;

    public static GameplayAnchorManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            anchors = new Dictionary<GameplayAnchorType, GameObject> {
            { GameplayAnchorType.Top, anchorTop },
            { GameplayAnchorType.Middle, anchorMiddle },
            { GameplayAnchorType.Bottom, anchorBottom }
        };
        } else {
            Destroy(gameObject);
        }
    }

    public void MoveToAnchor(GameplayAnchorType anchorType) {
        if (anchors.TryGetValue(anchorType, out GameObject targetAnchor)) {
            container.transform.position = targetAnchor.transform.position;
        } else {
            Debug.LogWarning($"Anchor {anchorType} not defined!");
        }
    }

    public void ShowContainer(bool show) {
        container.SetActive(show);
    }

    public void MoveContainerToAnchor(GameplayAnchorType anchorType, float transitionTime, System.Action onComplete) {
        if (anchors.TryGetValue(anchorType, out GameObject targetAnchor)) {
            StartCoroutine(AnimateMovement(targetAnchor, transitionTime, onComplete));
        } else {
            Debug.LogWarning($"Anchor {anchorType} not defined!");
        }
    }

    private IEnumerator AnimateMovement(GameObject targetAnchor, float transitionTime, System.Action onComplete) {
        float timeElapsed = 0;
        Vector3 startPosition = container.transform.position;
        Vector3 targetPosition = targetAnchor.transform.position;

        while (timeElapsed < transitionTime) {
            container.transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / transitionTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        container.transform.position = targetPosition;
        onComplete?.Invoke();
    }
}

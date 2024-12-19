using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BackgroundManager : MonoBehaviour {
    public static BackgroundManager Instance;
    public Image background;
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    private void Awake() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    public void ChangeBackground(string backgroundName)
    {
        if (string.IsNullOrEmpty(backgroundName))
        {
            Debug.LogError("Image name is null or empty!");
            return;
        }
        StartCoroutine(FadeOutAndIn(backgroundName));
    }

    // Coroutine para realizar o fade e troca de fundo
    private IEnumerator FadeOutAndIn(string backgroundName)
    {
        // Fade out do fundo atual
        yield return StartCoroutine(Fade(0f));

        // Carregar o novo fundo
        string imagePathPrefix = "Images/Backgrounds/";
        string fullPath = imagePathPrefix + backgroundName;

        Sprite loadedSprite = Resources.Load<Sprite>(fullPath);

        if (loadedSprite != null)
        {
            background.sprite = loadedSprite;
            background.CrossFadeAlpha(1f, 0f, true); // Aparecer instantaneamente
            Debug.Log($"Image '{backgroundName}' successfully loaded and applied.");
        }
        else
        {
            Debug.LogError($"Image '{backgroundName}' not found in Resources folder!");
        }

        // Fade in do novo fundo
        yield return StartCoroutine(Fade(1f));
    }

    // Função para realizar o fade
    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}

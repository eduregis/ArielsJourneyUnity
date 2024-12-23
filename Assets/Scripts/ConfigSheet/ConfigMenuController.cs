using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ConfigMenuController : MonoBehaviour
{
    public SliderComponentController ambienceVolumeSlider;
    public SliderComponentController musicVolumeSlider;
    public SliderComponentController effectVolumeSlider;

    private void Start() {
        // Configurar os sliders com valores iniciais e funções de fallback
        ambienceVolumeSlider.SetLabel("Vol. do ambiente:");
        ambienceVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.AmbienceVolumeKey));
        ambienceVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.AmbienceVolumeKey, value);

        musicVolumeSlider.SetLabel("Volume da música:");
        musicVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.MusicVolumeKey));
        musicVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.MusicVolumeKey, value);

        effectVolumeSlider.SetLabel("Vol. dos efeitos:");
        effectVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.EffectVolumeKey));
        effectVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.EffectVolumeKey, value);
    }

    private void OnVolumeChanged(string soundType, float value) {
        AudioManager.Instance.SetVolume(soundType, value);
        GameDataManager.SetVolume(soundType, value);
    }

    public void RestartGame() {
        GameDataManager.SetStage(0);
        GameDataManager.SetDialogue(0);
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "GameplayScene") {
            SceneManager.LoadScene(currentScene);
        }
    }
}

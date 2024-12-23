using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigMenuController : MonoBehaviour
{
    public SliderComponentController ambienceVolumeSlider;
    public SliderComponentController musicVolumeSlider;
    public SliderComponentController effectVolumeSlider;

    private void Start() {
        // Configurar os sliders com valores iniciais e funções de fallback
        ambienceVolumeSlider.SetLabel("Ambience Volume:");
        ambienceVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.AmbienceVolumeKey)); // Recuperar valor salvo
        ambienceVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.AmbienceVolumeKey, value);

        musicVolumeSlider.SetLabel("Music Volume:");
        musicVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.MusicVolumeKey)); // Recuperar valor salvo
        musicVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.MusicVolumeKey, value);

        effectVolumeSlider.SetLabel("Effect Volume:");
        effectVolumeSlider.SetSliderValue(GameDataManager.GetVolume(GameDataManager.EffectVolumeKey)); // Recuperar valor salvo
        effectVolumeSlider.onValueChangedFallback = value => OnVolumeChanged(GameDataManager.EffectVolumeKey, value);
    }

    private void OnVolumeChanged(string soundType, float value) {
        AudioManager.Instance.SetVolume(soundType, value);
        GameDataManager.SetVolume(soundType, value);
    }
}

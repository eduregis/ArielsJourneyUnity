using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderComponentController : MonoBehaviour {
    public Slider slider;
    public TMP_Text label;
    public System.Action<float> onValueChangedFallback;

    private void Start() {
        if (slider != null) {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnSliderValueChanged(float value) {
        onValueChangedFallback?.Invoke(value);
    }

    public void SetLabel(string text) {
        if (label != null) {
            label.text = text;
        }
    }

    public void SetSliderValue(float value) {
        if (slider != null) {
            slider.value = value;
        }
    }
}

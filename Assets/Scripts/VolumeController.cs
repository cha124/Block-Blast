using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public Slider slider;
    public AudioMixer mixer;
    public string volumeParameter = "MasterVolume";

    void Awake()
    {
        // 保存値を取得（なければ1）
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // UIに反映
        slider.SetValueWithoutNotify(volume);

        // 音量に反映
        ApplyVolume(volume);
    }

    public void OnSliderChanged(float value)
    {
        Debug.Log(value);
        ApplyVolume(value);
        PlayerPrefs.SetFloat("Volume", value);
    }

    void ApplyVolume(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        mixer.SetFloat(volumeParameter, Mathf.Log10(v) * 20);
    }
}
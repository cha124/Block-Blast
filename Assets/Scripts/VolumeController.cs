using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public Slider slider;


    void Awake()
    {
        // 保存値を取得（なければ1）
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        // UIに反映
        slider.SetValueWithoutNotify(volume);
    }

    public void OnSliderChanged(float value)
    {
        Debug.Log(value);
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}
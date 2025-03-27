using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioPanel : BasePanel
{
    public Button backButton;
    public AudioMixer audioMixer;

    [System.Serializable]
    public class SliderBar
    {
        public Slider slider;
        public string volumeName;
    }
    public List<SliderBar> sliderBars = new List<SliderBar>();

    // Start is called before the first frame update
    void Start()
    {
        backButton.onClick.AddListener(BackButtonEvent);

        foreach (var volumeSlider in sliderBars)
        {
            volumeSlider.slider.value = Mathf.Clamp(volumeSlider.slider.value, 0.0001f, 1f);

            float currentVolume = 0;
            if(audioMixer.GetFloat(volumeSlider.volumeName, out currentVolume))
            {
                volumeSlider.slider.value = Mathf.Pow(10, currentVolume / 20);
            }
            else
            {
                volumeSlider.slider.value = 1f;
            }

            volumeSlider.slider.onValueChanged.AddListener((value) =>
            {
                if (value < 0.0001f)
                {
                    // 如果值太小，直接设置为最小 dB 值
                    audioMixer.SetFloat(volumeSlider.volumeName, -80f);
                }
                else
                {
                    // 正常的对数转换
                    audioMixer.SetFloat(volumeSlider.volumeName, Mathf.Log10(value) * 20);
                }
            } );
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BackButtonEvent()
    {
        UIManager.Instance.ClosePanel(UIConst.AudioPanel);
        UIManager.Instance.OpenPanel(UIConst.SettingsPanel);
    }
}

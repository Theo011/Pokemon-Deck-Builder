using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixer button_press;
    public Slider slider;
    
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volume", -15);
        mixer.SetFloat("volume", slider.value);
        button_press.SetFloat("volume", slider.value);
    }

    private void Update()
    {
        if (slider.value != PlayerPrefs.GetFloat("volume"))
        {
            PlayerPrefs.SetFloat("volume", slider.value);
            mixer.SetFloat("volume", slider.value);
            button_press.SetFloat("volume", slider.value);
        }
    }

    public void Set_volume(float volume)
    {
        mixer.SetFloat("volume", volume);
        button_press.SetFloat("volume", volume);
    }
}
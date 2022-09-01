using UnityEngine;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixer button_press;

    private void Start()
    {
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume", -15));
        button_press.SetFloat("volume", PlayerPrefs.GetFloat("volume", -15));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
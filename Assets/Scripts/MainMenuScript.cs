using UnityEngine;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("volume", -15));
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
using UnityEngine;

public class ButtonPressScript : MonoBehaviour
{
    public AudioSource button_press;

    public void On_press()
    {
        button_press.Play();
    }
}
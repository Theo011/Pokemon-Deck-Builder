using UnityEngine;

public class NameFieldScript : MonoBehaviour
{
    public TMPro.TMP_InputField input_text;

    void Start()
    {
        input_text.text = PlayerPrefs.GetString("name", "");
    }

    void Update()
    {
        if (input_text.text != PlayerPrefs.GetString("name"))
        {
            PlayerPrefs.SetString("name", input_text.text);
        }
    }
}
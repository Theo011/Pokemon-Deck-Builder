using UnityEngine;

public class CardCountScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    
    void Update()
    {
        text.text = CardScript.card_count.ToString();
    }
}
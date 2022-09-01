using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI card_text;
    static public float height_count = 0.0f;
    static public int card_count = 0;

    public void On_click()
    {
        // Debug.Log(name);
        if (card_count < 20)
        {
            height_count += 60;
            GameObject new_text = Instantiate(card_text.gameObject, card_text.transform.position, card_text.transform.rotation);
            new_text.transform.SetParent(card_text.transform.parent);
            new_text.transform.position = new Vector3(card_text.transform.position.x, card_text.transform.position.y - height_count, card_text.transform.position.z);
            new_text.GetComponent<TMPro.TextMeshProUGUI>().text = name;
            new_text.GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
            new_text.GetComponent<Button>().enabled = true;
            new_text.name = name;
            card_count++;
        }
    }

    void OnDestroy()
    {
        height_count = 0.0f;
        card_count = 0;
    }
}
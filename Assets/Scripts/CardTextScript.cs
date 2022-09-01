using UnityEngine;

public class CardTextScript : MonoBehaviour
{
    public void On_click()
    {
        CardScript.card_count--;
        CardScript.height_count -= 80;
        Destroy(gameObject);
    }
}
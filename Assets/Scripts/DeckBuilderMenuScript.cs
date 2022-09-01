using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilderMenuScript : MonoBehaviour
{
    static public int deck_slot;

    public void Slot_1()
    {
        deck_slot = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Slot_2()
    {
        deck_slot = 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Slot_3()
    {
        deck_slot = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
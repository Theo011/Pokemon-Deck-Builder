using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilderBackButton : MonoBehaviour
{
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
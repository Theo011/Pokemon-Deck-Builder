using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
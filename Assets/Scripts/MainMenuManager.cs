using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void ZacznijGre()
    {
        SceneManager.LoadScene("Home");
    }

    public void WyjdzZ_Gry()
    {
        Application.Quit();
        Debug.Log("Gra została zamknięta");
    }
}
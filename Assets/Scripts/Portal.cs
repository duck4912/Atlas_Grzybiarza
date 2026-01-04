using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string NAZWA_SCENY;   
    public string NAZWA_SPAWNU;  

    public static string PUNKT_DOCELOWY;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PUNKT_DOCELOWY = NAZWA_SPAWNU;
            SceneManager.LoadScene(NAZWA_SCENY);
        }
    }
}
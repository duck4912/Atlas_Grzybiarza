using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string NAZWA_SCENY;   
    public string NAZWA_SPAWNU;  

    public static string PUNKT_DOCELOWY;

    // --- DODAJEMY TO ---
    private float timer = 0f;
    private float blokadaCzasu = 0.5f; // pół sekundy przerwy
    // -------------------

    void Update()
    {
        // Licznik czasu rośnie od momentu załadowania sceny
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy: czy to gracz ORAZ czy minęło już pół sekundy?
        if (other.CompareTag("Player") && timer > blokadaCzasu)
        {
            PUNKT_DOCELOWY = NAZWA_SPAWNU;
            SceneManager.LoadScene(NAZWA_SCENY);
        }
    }
}
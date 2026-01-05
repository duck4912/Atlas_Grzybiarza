using UnityEngine;

public class PickableMushroom : MonoBehaviour
{
    public string mushroomName; // Tu wpiszesz np. "Borowik" lub "Muchomor"

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Znalazłeś: " + mushroomName);
            // Tutaj wywołamy dodawanie do Twojego Atlasu
            Destroy(gameObject); 
        }
    }
}
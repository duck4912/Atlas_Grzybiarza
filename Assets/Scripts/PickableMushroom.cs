using UnityEngine;

public class PickableMushroom : MonoBehaviour
{
    public string mushroomName; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Znalazłeś: " + mushroomName);
            Destroy(gameObject); 
        }
    }
}
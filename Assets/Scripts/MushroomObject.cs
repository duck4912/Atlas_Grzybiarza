using UnityEngine;

public class MushroomObject : MonoBehaviour
{
    public MushroomData data;
    public GameObject pickupPopup; 
    
    [Header("Audio Settings")]
    public AudioClip pickupSound; 
    [Range(0f, 1f)]
    public float volume = 0.5f;   

    private bool canPickUp = false;

    [HideInInspector] 
    public SceneSpawner mySpawner; 

    void Start()
    {
        if(pickupPopup != null) 
        {
            pickupPopup.SetActive(false);
        }
    }

    void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = true;
            if(pickupPopup != null) pickupPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canPickUp = false;
            if(pickupPopup != null) pickupPopup.SetActive(false);
        }
    }

    void PickUp()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.CollectMushroom(data);

        if (mySpawner != null) 
        {
            mySpawner.MushroomWasPickedUp(this.gameObject);
        }

        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);
        }

        Destroy(gameObject);
    }
}
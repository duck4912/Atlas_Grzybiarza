using UnityEngine;

public class MushroomObject : MonoBehaviour
{
    public MushroomData data;
    public GameObject pickupPopup; // The UI Canvas ("Press E")
    
    private bool canPickUp = false;

    // This is hidden because the Spawner assigns it automatically
    [HideInInspector] 
    public SceneSpawner mySpawner; 


    void Start()
    {
        // Force the popup to be invisible when the mushroom spawns
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
        // 1. Tell Manager we found a new type
        if(GameManager.Instance != null)
            GameManager.Instance.CollectMushroom(data);

        // 2. CRITICAL: Tell the Spawner to remove this specific mushroom from the "Survivor List"
        if (mySpawner != null) 
        {
            mySpawner.MushroomWasPickedUp(this.gameObject);
        }

        // 3. Destroy visual object
        Destroy(gameObject);
    }
}
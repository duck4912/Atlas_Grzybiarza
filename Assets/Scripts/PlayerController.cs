using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void Start()
    {
        // Teraz ziutek szuka nowej, wspólnej pamięci w klasie Portal
        if (!string.IsNullOrEmpty(Portal.PUNKT_DOCELOWY))
        {
            GameObject spawnPoint = GameObject.Find(Portal.PUNKT_DOCELOWY);
            
            if (spawnPoint != null)
            {
                transform.position = spawnPoint.transform.position;
                if(rb != null) {
                    rb.position = spawnPoint.transform.position;
                    rb.linearVelocity = Vector2.zero;
                }
            }
        }
    }
}
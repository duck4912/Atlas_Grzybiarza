using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator; 
    
    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); 

        if (movement.x > 0) {
            transform.localScale = new Vector3(2f, 2f, 1f);
        } 
        else if (movement.x < 0) {
            transform.localScale = new Vector3(-2f, 2f, 1f);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void Start()
    {
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
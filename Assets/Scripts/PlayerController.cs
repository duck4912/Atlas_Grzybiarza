using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator; // <--- DODANE: Przeciągnij tu obiekt Player w Inspektorze
    
    Vector2 movement;

    void Update()
    {
        // Odczyt wejścia
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // --- SEKRETY ANIMACJI ---
        // Wysyłamy dane do parametrów, które stworzyłaś w Animatorze
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // sqrMagnitude sprawdza, czy się ruszamy

        // --- ODWRACANIE (FLIP) ---
        // Używamy localScale zamiast flipX, żeby odwrócić też przyklejoną twarz!
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
        // Twoja dotychczasowa logika teleportacji (zostawiamy bez zmian)
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
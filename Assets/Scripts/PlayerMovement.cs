using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public AudioSource walkingSound; // Drag the AudioSource here in Inspector

    Vector2 movement;

    void Update()
    {
        // 1. Get Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Handle Audio
        // check if we are actually pressing keys
        if (movement.x != 0 || movement.y != 0) 
        {
            // If we are moving, but the sound is NOT on yet -> Turn it on
            if (!walkingSound.isPlaying)
            {
                walkingSound.Play();
            }
        }
        else
        {
            // If we stopped moving, but the sound IS still on -> Turn it off
            if (walkingSound.isPlaying)
            {
                walkingSound.Stop();
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
using UnityEngine;

public class GrandpaNPC : MonoBehaviour
{
    public GameObject dialoguePrompt; // The floating "Press E" text object
    private bool playerInRange = false;
    private bool isTalking = false;   // Keeps track: Are we reading? Or walking?

    void Start()
    {
        // 1. If we return to this scene later and already have the book, 
        // Grandpa should be gone.
        if (GameManager.Instance != null && GameManager.Instance.hasBook)
        {
            Destroy(gameObject);
            return;
        }

        // Hide prompt by default
        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                // FIRST CLICK: Open Dialog
                StartConversation();
            }
            else
            {
                // SECOND CLICK: Close Dialog + Give Book + Vanish
                EndConversationAndGiveBook();
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        
        // Hide "Press E" so it doesn't block the view
        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);

        // Tell the Global UI to show the blue box
        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.ShowDialog("It's dangerous to go alone! Take this Mushroom Guide.");
        }
    }

    void EndConversationAndGiveBook()
    {
        // 1. Close the dialog box
        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.HideDialog();
            
            // 2. Show the top notification
            CollectionBookUI.Instance.ShowNotification("Received Mushroom Guide! (Press TAB)", 4.0f);
        }

        // 3. Update Game Data
        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasBook = true;
        }

        // 4. STOP INTERACTION (But don't destroy him yet!)
        // This turns off this script, so pressing 'E' does nothing anymore.
        // He will remain visible until you leave the scene and come back.
        this.enabled = false; 
    }

    // --- PHYSICS TRIGGERS ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // Only show "Press E" if we aren't already talking
            if (dialoguePrompt != null && !isTalking) dialoguePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
            
            // Safety: If player runs away while talking, close the box
            if (isTalking && CollectionBookUI.Instance != null)
            {
                CollectionBookUI.Instance.HideDialog();
                isTalking = false;
            }
        }
    }
}
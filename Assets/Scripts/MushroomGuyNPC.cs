using UnityEngine;

public class MushroomGuyNPC : MonoBehaviour
{
    public GameObject dialoguePrompt; // The "E" bubble container
    private bool playerInRange = false;
    private bool isTalking = false;

    void Start()
    {
        // 1. CHECK FOR VICTORY
        if (GameManager.Instance != null)
        {
            // Use the helper function we made in GameManager
            int current = GameManager.Instance.GetUniqueMushroomCount();
            int total = GameManager.Instance.allPossibleMushrooms.Count;

            // If we haven't found everything, HIDE THIS GUY immediately.
            if (current < total)
            {
                gameObject.SetActive(false); 
                return;
            }
        }

        // If we survived the check above, it means we WON!
        // Just ensure the "E" prompt is hidden to start.
        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                // First E: Talk
                StartEndingDialog();
            }
            else
            {
                // Second E: Fade Out
                FinishGame();
            }
        }
    }

    void StartEndingDialog()
    {
        isTalking = true;
        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);

        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.ShowDialog("Thank you for taking care of the forest... Your uncle would be proud.");
        }
    }

    void FinishGame()
    {
        // 1. Close the dialog box
        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.HideDialog();
            
            // 2. START THE FADE OUT
            CollectionBookUI.Instance.TriggerEndingSequence();
        }
    }

    // --- PHYSICS TRIGGERS ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (dialoguePrompt != null && !isTalking) dialoguePrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
            
            // If we leave while talking, reset (optional)
            if (isTalking && CollectionBookUI.Instance != null)
            {
                CollectionBookUI.Instance.HideDialog();
                isTalking = false;
            }
        }
    }
}
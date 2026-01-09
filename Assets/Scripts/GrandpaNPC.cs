using UnityEngine;

public class GrandpaNPC : MonoBehaviour
{
    public GameObject dialoguePrompt; 
    private bool playerInRange = false;
    private bool isTalking = false;   

    void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.hasBook)
        {
            Destroy(gameObject);
            return;
        }

        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartConversation();
            }
            else
            {
                EndConversationAndGiveBook();
            }
        }
    }

    void StartConversation()
    {
        isTalking = true;
        
        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);

        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.ShowDialog("It's dangerous to go alone! Take this Mushroom Guide.");
        }
    }

    void EndConversationAndGiveBook()
    {
        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.HideDialog();
            
            CollectionBookUI.Instance.ShowNotification("Received Mushroom Guide! (Press TAB)", 4.0f);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.hasBook = true;
        }

        this.enabled = false; 
    }

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
            
            if (isTalking && CollectionBookUI.Instance != null)
            {
                CollectionBookUI.Instance.HideDialog();
                isTalking = false;
            }
        }
    }
}
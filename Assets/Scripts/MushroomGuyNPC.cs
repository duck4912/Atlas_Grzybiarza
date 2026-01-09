using UnityEngine;

public class MushroomGuyNPC : MonoBehaviour
{
    public GameObject dialoguePrompt; 
    private bool playerInRange = false;
    private bool isTalking = false;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            int current = GameManager.Instance.GetUniqueMushroomCount();
            int total = GameManager.Instance.allPossibleMushrooms.Count;

            if (current < total)
            {
                gameObject.SetActive(false); 
                return;
            }
        }

        if (dialoguePrompt != null) dialoguePrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isTalking)
            {
                StartEndingDialog();
            }
            else
            {
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
        if (CollectionBookUI.Instance != null)
        {
            CollectionBookUI.Instance.HideDialog();
            
            CollectionBookUI.Instance.TriggerEndingSequence();
        }
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
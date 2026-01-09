using UnityEngine;
using TMPro; 
using System.Collections.Generic;
using System.Collections; 

public class CollectionBookUI : MonoBehaviour
{
    public static CollectionBookUI Instance;

    [Header("Book Settings")]
    public GameObject bookPanel;       
    public Transform gridContainer;    
    public GameObject slotPrefab;      

    [Header("Notification Settings")]
    public GameObject notificationPanel;       
    public TextMeshProUGUI notificationText;   

    [Header("Dialog Settings")]
    public GameObject dialogPanel;     
    public TextMeshProUGUI dialogText; 

    [Header("End Game Settings")]
    public GameObject endScreenPanel;        
    public CanvasGroup endScreenCanvasGroup; 

    private bool isBookOpen = false;
    private List<GameObject> activeSlots = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (bookPanel != null) bookPanel.SetActive(false);
        if (notificationPanel != null) notificationPanel.SetActive(false);
        if (dialogPanel != null) dialogPanel.SetActive(false);
        
        if (endScreenPanel != null) endScreenPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.Instance != null && GameManager.Instance.hasBook)
            {
                ToggleBook();
            }
        }
    }

    public void ToggleBook()
    {
        isBookOpen = !isBookOpen;
        bookPanel.SetActive(isBookOpen);

        if (isBookOpen)
        {
            RefreshUI();
        }
    }

    void RefreshUI()
    {
        foreach (GameObject slot in activeSlots) Destroy(slot);
        activeSlots.Clear();

        if (GameManager.Instance == null) return;

        foreach (var data in GameManager.Instance.allPossibleMushrooms)
        {
            GameObject newSlot = Instantiate(slotPrefab, gridContainer);
            activeSlots.Add(newSlot);

            bool found = GameManager.Instance.HasCollected(data);
            
            MushroomSlotUI ui = newSlot.GetComponent<MushroomSlotUI>();
            if (ui != null) ui.Setup(data, found);
        }
    }

    public void ShowNotification(string message, float duration)
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(true);
            notificationText.text = message;
            CancelInvoke("HideNotification");
            Invoke("HideNotification", duration);
        }
    }

    void HideNotification()
    {
        if (notificationPanel != null) notificationPanel.SetActive(false);
    }

    public void ShowDialog(string text)
    {
        if (dialogPanel != null)
        {
            dialogPanel.SetActive(true);
            dialogText.text = text;
        }
    }

    public void HideDialog()
    {
        if (dialogPanel != null) dialogPanel.SetActive(false);
    }

    public void TriggerEndingSequence()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);
            
            if (endScreenCanvasGroup != null)
            {
                endScreenCanvasGroup.alpha = 0; 
                StartCoroutine(FadeInEndScreen());
            }
        }
    }

    IEnumerator FadeInEndScreen()
    {
        float duration = 2.0f; 
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = time / duration;
            endScreenCanvasGroup.alpha = alpha;
            yield return null;
        }
        
        endScreenCanvasGroup.alpha = 1; 
        Debug.Log("The End.");
    }
}
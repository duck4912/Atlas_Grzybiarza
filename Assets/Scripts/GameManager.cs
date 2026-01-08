using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // --- 1. SINGLETON SETUP ---
    public static GameManager Instance;

    [Header("Game Data")]
    // Drag ALL your mushroom data files here in the Inspector!
    public List<MushroomData> allPossibleMushrooms; 

    // --- BOOK TRACKING ---
    public bool hasBook = false; // Starts false, Grandpa turns it true

    // Tracks which unique TYPES you have found
    private HashSet<MushroomData> collectedMushrooms = new HashSet<MushroomData>();

    // --- 2. SAVE SYSTEM DATA ---
    public Dictionary<string, List<MushroomSaveData>> sceneState = new Dictionary<string, List<MushroomSaveData>>();

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

    // --- 3. COLLECTION LOGIC ---
    public void CollectMushroom(MushroomData data)
    {
        // Only count it if we haven't found this type before
        if (!collectedMushrooms.Contains(data))
        {
            collectedMushrooms.Add(data);

            int currentCount = collectedMushrooms.Count;
            int totalCount = allPossibleMushrooms.Count;

            Debug.Log($"NEW DISCOVERY! Found {data.mushroomName}");
            Debug.Log($"Collection Status: {currentCount} / {totalCount}");
            
            // --- CHECK FOR VICTORY ---
            if(currentCount >= totalCount)
            {
                Debug.Log("CONGRATULATIONS! You found every mushroom in the game!");

                // NEW: Tell the UI to show the "Go Home" message
                if (CollectionBookUI.Instance != null)
                {
                    CollectionBookUI.Instance.ShowNotification("Collection Complete! Return Home.", 6.0f);
                }
            }
        }
        else
        {
            Debug.Log($"Picked up another {data.mushroomName}. Total count unchanged.");
        }
    }

    // --- UI HELPER ---
    // The UI asks this to know if it should color the icon
    public bool HasCollected(MushroomData data)
    {
        return collectedMushrooms.Contains(data);
    }

    public int GetUniqueMushroomCount()
    {
        return collectedMushrooms.Count;
    }

    // --- 4. SAVE & LOAD LOGIC ---
    public void SaveZoneData(string spawnerID, List<MushroomSaveData> data)
    {
        if (sceneState.ContainsKey(spawnerID)) sceneState[spawnerID] = data;
        else sceneState.Add(spawnerID, data);
        
        Debug.Log($"GameManager: Saved state for zone '{spawnerID}'. {data.Count} mushrooms survived.");
    }

    public List<MushroomSaveData> LoadZoneData(string spawnerID)
    {
        if (sceneState.ContainsKey(spawnerID)) return sceneState[spawnerID];
        return null;
    }
}

// --- 5. HELPER CLASS ---
[System.Serializable]
public class MushroomSaveData
{
    public Vector3 position;
    public MushroomData data;

    public MushroomSaveData(Vector3 pos, MushroomData type)
    {
        position = pos;
        data = type;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // --- 1. SINGLETON SETUP ---
    public static GameManager Instance;

    [Header("Game Data")]
    // Drag ALL your mushroom data files here in the Inspector!
    // This allows us to calculate "X out of Y found"
    public List<MushroomData> allPossibleMushrooms; 

    // Tracks which unique TYPES you have found (for the "Pokedex" count)
    private HashSet<MushroomData> collectedMushrooms = new HashSet<MushroomData>();

    // --- 2. SAVE SYSTEM DATA ---
    // Stores the survivors for each spawner. 
    // Key = Spawner ID (string), Value = List of surviving mushrooms
    public Dictionary<string, List<MushroomSaveData>> sceneState = new Dictionary<string, List<MushroomSaveData>>();

    void Awake()
    {
        // Singleton Logic: Ensure only one Manager exists and it survives scene loads
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If a duplicate tries to spawn in a new scene, destroy it
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

            // Calculate Progress
            int currentCount = collectedMushrooms.Count;
            int totalCount = allPossibleMushrooms.Count;

            Debug.Log($"NEW DISCOVERY! Found {data.mushroomName}");
            Debug.Log($"Collection Status: {currentCount} / {totalCount}");
            
            if(currentCount >= totalCount)
            {
                Debug.Log("CONGRATULATIONS! You found every mushroom in the game!");
            }
        }
        else
        {
            Debug.Log($"Picked up another {data.mushroomName}. Total count unchanged.");
        }
    }

    public int GetUniqueMushroomCount()
    {
        return collectedMushrooms.Count;
    }

    // --- 4. SAVE & LOAD LOGIC (Called by Spawners) ---
    
    // Called by a Spawner when the player leaves the scene (or game closes)
    public void SaveZoneData(string spawnerID, List<MushroomSaveData> data)
    {
        if (sceneState.ContainsKey(spawnerID))
        {
            sceneState[spawnerID] = data; // Update existing record
        }
        else
        {
            sceneState.Add(spawnerID, data); // Create new record
        }
        Debug.Log($"GameManager: Saved state for zone '{spawnerID}'. {data.Count} mushrooms survived.");
    }

    // Called by a Spawner when the scene starts
    public List<MushroomSaveData> LoadZoneData(string spawnerID)
    {
        if (sceneState.ContainsKey(spawnerID))
        {
            return sceneState[spawnerID]; // Return the saved survivors
        }
        return null; // Return null if we've never been here before
    }
}

// --- 5. HELPER CLASS ---
// This defines what "Saved Data" looks like for a single mushroom
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
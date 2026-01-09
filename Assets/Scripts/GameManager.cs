using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Data")]
    public List<MushroomData> allPossibleMushrooms; 

    public bool hasBook = false; 

    private HashSet<MushroomData> collectedMushrooms = new HashSet<MushroomData>();

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

    public void CollectMushroom(MushroomData data)
    {
        if (!collectedMushrooms.Contains(data))
        {
            collectedMushrooms.Add(data);

            int currentCount = collectedMushrooms.Count;
            int totalCount = allPossibleMushrooms.Count;

            Debug.Log($"NEW DISCOVERY! Found {data.mushroomName}");
            Debug.Log($"Collection Status: {currentCount} / {totalCount}");
            
            if(currentCount >= totalCount)
            {
                Debug.Log("CONGRATULATIONS! You found every mushroom in the game!");

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

    public bool HasCollected(MushroomData data)
    {
        return collectedMushrooms.Contains(data);
    }

    public int GetUniqueMushroomCount()
    {
        return collectedMushrooms.Count;
    }

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
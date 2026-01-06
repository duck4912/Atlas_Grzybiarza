using UnityEngine;
using System.Collections.Generic;

public class SceneSpawner : MonoBehaviour
{
    [Header("Persistence ID")]
    public string spawnerID; 

    [Header("Settings")]
    public List<MushroomData> possibleMushrooms;
    public int spawnCount = 8;
    
    [Header("Spawn Zones")]
    public List<Collider2D> spawnAreas; 
    public LayerMask obstacleLayer;

    private class TrackedMushroom
    {
        public GameObject obj;      
        public MushroomData data;   
        public Vector3 originalPos; 
    }

    private List<TrackedMushroom> activeMushrooms = new List<TrackedMushroom>();

    void Start()
    {
        activeMushrooms.Clear();
        var savedData = GameManager.Instance.LoadZoneData(spawnerID);

        if (savedData != null)
        {
            RestoreMushrooms(savedData);
        }
        else
        {
            SpawnFreshMushrooms();
        }
    }

    void RestoreMushrooms(List<MushroomSaveData> dataList)
    {
        foreach (var saved in dataList)
        {
            CreateMushroom(saved.data, saved.position);
        }
    }

    // --- NEW LOGIC: "BAG" SPAWNING ---
    void SpawnFreshMushrooms()
    {
        if (possibleMushrooms.Count == 0) return;

        // 1. Create the Bag
        List<MushroomData> spawnQueue = new List<MushroomData>();

        // 2. Add "Must Haves" (One of each type to guarantee variety)
        foreach (var type in possibleMushrooms)
        {
            // Only add if we haven't hit the limit yet
            if (spawnQueue.Count < spawnCount)
            {
                spawnQueue.Add(type);
            }
        }

        // 3. Fill the rest of the slots with Random picks
        while (spawnQueue.Count < spawnCount)
        {
            MushroomData randomType = possibleMushrooms[Random.Range(0, possibleMushrooms.Count)];
            spawnQueue.Add(randomType);
        }

        // 4. Spawn everything in the Bag
        foreach (var dataToSpawn in spawnQueue)
        {
            TrySpawnSpecificMushroom(dataToSpawn);
        }
    }

    // Renamed from TrySpawnRandomMushroom to accept a specific type
    void TrySpawnSpecificMushroom(MushroomData dataToSpawn)
    {
        if (spawnAreas.Count == 0) return;

        for (int attempt = 0; attempt < 30; attempt++)
        {
            Collider2D currentArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
            Bounds bounds = currentArea.bounds;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 randomPoint = new Vector2(randomX, randomY);

            if (currentArea.OverlapPoint(randomPoint))
            {
                if (Physics2D.OverlapCircle(randomPoint, 0.3f, obstacleLayer) == null)
                {
                    // USE THE SPECIFIC DATA WE PASSED IN
                    CreateMushroom(dataToSpawn, randomPoint);
                    return;
                }
            }
        }
        Debug.LogWarning($"Could not find a spot for {dataToSpawn.mushroomName} after 30 attempts.");
    }

    void CreateMushroom(MushroomData data, Vector3 pos)
    {
        GameObject newMushroom = Instantiate(data.prefab, pos, Quaternion.identity);
        
        MushroomObject script = newMushroom.GetComponent<MushroomObject>();
        if(script != null) script.mySpawner = this;

        TrackedMushroom tracker = new TrackedMushroom();
        tracker.obj = newMushroom;
        tracker.data = data;
        tracker.originalPos = pos; 

        activeMushrooms.Add(tracker);
    }

    public void MushroomWasPickedUp(GameObject mushroomObj)
    {
        for (int i = activeMushrooms.Count - 1; i >= 0; i--)
        {
            if (activeMushrooms[i].obj == mushroomObj)
            {
                activeMushrooms.RemoveAt(i);
                return;
            }
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance == null) return;

        List<MushroomSaveData> survivors = new List<MushroomSaveData>();

        foreach (var tracker in activeMushrooms)
        {
            MushroomSaveData info = new MushroomSaveData(tracker.originalPos, tracker.data);
            survivors.Add(info);
        }

        GameManager.Instance.SaveZoneData(spawnerID, survivors);
    }
}
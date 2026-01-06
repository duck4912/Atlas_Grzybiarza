using UnityEngine;

[CreateAssetMenu(fileName = "New Mushroom", menuName = "Mushroom Data")]
public class MushroomData : ScriptableObject
{
    public string mushroomName; // e.g., "Borowik"
    public Sprite icon;         // The image for the book
    public GameObject prefab;   // The Borowik prefab
}
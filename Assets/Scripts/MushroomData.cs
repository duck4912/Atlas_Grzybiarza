using UnityEngine;

[CreateAssetMenu(fileName = "New Mushroom", menuName = "Mushroom Data")]
public class MushroomData : ScriptableObject
{
    public string mushroomName; 
    public Sprite icon;         
    public GameObject prefab;   
}
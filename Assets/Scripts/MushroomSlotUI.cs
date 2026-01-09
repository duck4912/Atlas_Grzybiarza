using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class MushroomSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    private Color lockedColor = new Color(0f, 0f, 0f, 1f); 
    private Color unlockedColor = Color.white;             

    public void Setup(MushroomData data, bool isUnlocked)
    {
        if(data.icon != null) iconImage.sprite = data.icon;

        if (isUnlocked)
        {
            iconImage.color = unlockedColor;
            nameText.text = data.mushroomName;
        }
        else
        {
            iconImage.color = lockedColor;
            nameText.text = "???";
        }
    }
}
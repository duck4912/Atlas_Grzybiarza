using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class MushroomSlotUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI nameText;

    private Color lockedColor = new Color(0f, 0f, 0f, 1f); // Black Silhouette
    private Color unlockedColor = Color.white;             // Full Color

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
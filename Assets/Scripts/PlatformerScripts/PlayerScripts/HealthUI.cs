
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    Image displayHeart;
    
    public Sprite[] heartImages;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayHeart = GetComponent<Image>();
        displayHeart.sprite = heartImages[0];
    }

    public void spriteUpdate(float healthLeft)
    {
        healthLeft = Mathf.Clamp01(healthLeft);
        //takes in a percent of health, gets a corrisponding image, with heartImages[0] == 100% hp
        int index = Mathf.RoundToInt((1f - healthLeft) * (heartImages.Length - 1f));
        //index = Mathf.Clamp(index, 0, heartImages.Length - 1);
        
        displayHeart.sprite = heartImages[index];

    }
}

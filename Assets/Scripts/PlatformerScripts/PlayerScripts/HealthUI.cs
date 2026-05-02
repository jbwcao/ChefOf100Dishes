
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

    public void spriteUpdate(int healthLeft)
    {   
        // spaghetti
        displayHeart.sprite = heartImages[healthLeft % 4];
    }
}

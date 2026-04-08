using UnityEngine;
using System.Collections.Generic;
public class WantedDishUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDishes(List<Dish> dishes)
    {
        Debug.Log("call setdish");
        int i  = 0;
        foreach (Dish dish in dishes)
        {
            GameObject img = new GameObject(dish.name);
            img.transform.SetParent(transform);
            img.transform.localPosition = new Vector3(i * 1.1f, 0, 0);
            img.transform.localScale = new Vector3(3f, 3f, 1.0f);
            SpriteRenderer sr = img.AddComponent<SpriteRenderer>();
            sr.sprite = dish.sprite;
            i++;

        }
        

    }
}

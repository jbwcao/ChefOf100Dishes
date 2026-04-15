using UnityEngine;
using System.Collections.Generic;
public class WantedIngredientUI : MonoBehaviour
{
    public void SetIngredient(Ingredient ingredient)
    {
        Debug.Log("call setingredient");
    
        GameObject img = new GameObject(ingredient.name);
        img.transform.SetParent(transform);
        img.transform.localPosition = new Vector3(1.1f, 0, 0);
        img.transform.localScale = new Vector3(5f, 5f, 1.0f);
        SpriteRenderer sr = img.AddComponent<SpriteRenderer>();
        sr.sprite = ingredient.sprite;
    }
}

        
    
    /**  The code below generates MULTIPLE spaced out ingredients, in case we want multiple ingredient wants,
    this is old code from when we wanted multiple dishes so it may need more refactoring.

    public void SetIngredient(List<Ingredient> ingredients)
    {
        Debug.Log("call setingredient");
        int i  = 0;
        foreach (Ingredient i in ingredients)
        {
            GameObject img = new GameObject(i.name);
            img.transform.SetParent(transform);
            img.transform.localPosition = new Vector3(i * 1.1f, 0, 0);
            img.transform.localScale = new Vector3(3f, 3f, 1.0f);
            SpriteRenderer sr = img.AddComponent<SpriteRenderer>();
            sr.sprite = i.sprite;
            i++;

        }
        

    }*/


using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RecipeEntryUI : MonoBehaviour
{
    public Image dishImage;
    public TextMeshProUGUI dishName;
    public TextMeshProUGUI ingredients;

    public void Setup(Cookbook.Recipe recipe, bool discovered)
    {
        dishImage.sprite = recipe.dish.sprite;
        dishName.text = recipe.dish.name;
        
        if (discovered)
        {
            string ingredientText = "";
            foreach (Ingredient i in recipe.ingredients)
            {
                ingredientText += i.name + " ";
            }
            ingredients.text = ingredientText;
        }
        else
        {
            ingredients.text = "???";
        }
    }
}

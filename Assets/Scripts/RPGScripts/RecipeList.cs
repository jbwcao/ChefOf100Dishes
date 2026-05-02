using UnityEngine;
using System.Collections.Generic;

public class RecipeList : MonoBehaviour
{
    public Cookbook cookbook;
    public GameObject recipeEntryPrefab;
    public Transform grid;

    void OnEnable()
    {
        // Clear old entries
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
        
        // Populate
        foreach (Cookbook.Recipe recipe in cookbook.recipes)
        {
            GameObject entry = Instantiate(recipeEntryPrefab, grid);
            bool discovered = GameManager.Instance.discoveredDishes.Contains(recipe.dish.name);
            entry.GetComponent<RecipeEntryUI>().Setup(recipe, discovered);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "Cookbook", menuName = "Scriptable Objects/Cookbook")]
public class Cookbook : ScriptableObject {
    [System.Serializable]
    public class Recipe {
        [SerializeField] public Dish dish;
        
        [SerializeField] public List<Ingredient> ingredients;
    }
    [SerializeField] public List<Recipe> recipes;
    void OnEnable() {
        foreach (Recipe recipe in recipes) {
            recipe.ingredients.Sort((a, b) => String.Compare(a.name, b.name));
    }
        
    }
}

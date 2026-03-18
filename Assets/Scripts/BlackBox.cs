using UnityEngine;
using System.Collections.Generic;
using System;
public class BlackBox : MonoBehaviour {
    public Cookbook cookbook;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<Ingredient> droppedIngredient;
    void Start() {
        droppedIngredient = new List<Ingredient>();
    }

    // Update is called once per frame
    void Update() {
        
    }
    public string Cook() {
        droppedIngredient.Sort((a, b) => String.Compare(a.name, b.name));
       
        foreach (Cookbook.Recipe recipe in cookbook.recipes) {
            if (recipe.ingredients.Count == droppedIngredient.Count) {
                bool flag = true;
                
                for (int i = 0; i < recipe.ingredients.Count; i++ ) {
                    if (recipe.ingredients[i].name != droppedIngredient[i].name) {
                        flag = false;
                        break;
                    }
                }

                if (flag) {
                    droppedIngredient.Clear();
                    return recipe.dishname;
                }
            }
        }

        droppedIngredient.Clear();
        return "this is not a dish";

    }
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Ingredient")) {
            droppedIngredient.Add(coll.gameObject.GetComponent<IngredientObject>().ingredient);
            Destroy(coll.gameObject);
            
        }
    }
}

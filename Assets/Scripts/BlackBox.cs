using UnityEngine;
using System.Collections.Generic;
using System;
public class BlackBox : MonoBehaviour {
    public Cookbook cookbook;
    List<Ingredient> droppedIngredient;
    public Transform dishSpawn;
    public GameObject masterPrefab;
    void Start() {
        droppedIngredient = new List<Ingredient>();
    }

    public void Cook() {
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

                    Debug.Log(recipe.dish.name);

                    GameObject ingredientObj = Instantiate(masterPrefab, dishSpawn.position, dishSpawn.rotation);
                    MasterPrefab ingredientItem = ingredientObj.GetComponent<MasterPrefab>();
                    
                    ingredientObj.name = recipe.dish.name;
                    ingredientItem.sprite = recipe.dish.sprite;
                    ingredientItem.name = recipe.dish.name;
                    ingredientItem.dish = recipe.dish;
                }
            }
        }

        droppedIngredient.Clear();
        

    }
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Ingredient")) {
            droppedIngredient.Add(coll.gameObject.GetComponent<MasterPrefab>().ingredient);
            Destroy(coll.gameObject);
            foreach (Ingredient i in droppedIngredient)
            {
                Debug.Log(i.name);
            }
        }
    }
}

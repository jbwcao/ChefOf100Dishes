using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Blackerbox : MonoBehaviour {
    public Cookbook cookbook;
    List<Ingredient> droppedIngredient;
    public Transform dishSpawn;
    public GameObject masterPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> potDisplay = new List<GameObject>();
    void Start() {
        droppedIngredient = new List<Ingredient>();
    }

    public void Cook() {
        droppedIngredient.Sort((a, b) => String.Compare(a.name, b.name));
       
        foreach (Cookbook.Recipe recipe in cookbook.recipes) {
            Debug.Log(recipe.dish.name);
            if (recipe.ingredients.Count == droppedIngredient.Count) {
                bool flag = true;
                
                for (int i = 0; i < recipe.ingredients.Count; i++ ) {
                    if (recipe.ingredients[i].name != droppedIngredient[i].name) {
                        flag = false;
                        break;
                    }
                }

                if (flag) {

                    


                    Debug.Log(recipe.dish.name);

                    GameObject ingredientObj = Instantiate(masterPrefab, dishSpawn.position, dishSpawn.rotation);
                    MasterPrefab ingredientItem = ingredientObj.GetComponent<MasterPrefab>();
                    
                    ingredientObj.name = recipe.dish.name;
                    ingredientItem.sprite = recipe.dish.sprite;
                    ingredientItem.name = recipe.dish.name;
                    ingredientItem.dish = recipe.dish;
                    ingredientItem.ingredientList = new List<Ingredient>(droppedIngredient);

                    droppedIngredient.Clear();
                }
            }
        }

        droppedIngredient.Clear();
        for (int i = 0; i < spawnPoints.Count; i++) {
            spawnPoints[i].GetComponent<Image>().sprite = null;
            spawnPoints[i].SetActive(false);

        }
        

    }
    private void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Ingredient")) {
            droppedIngredient.Add(coll.gameObject.GetComponent<MasterPrefab>().ingredient);
            int index = droppedIngredient.Count - 1;
            spawnPoints[index].GetComponent<Image>().sprite = droppedIngredient[index].sprite;
            spawnPoints[index].SetActive(true);
            Destroy(coll.gameObject);
            foreach (Ingredient i in droppedIngredient)
            {
                Debug.Log(i.name);
            }
        }
    }
}

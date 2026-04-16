using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;

public class Blackerbox : MonoBehaviour {
    public Cookbook cookbook;
    List<Ingredient> droppedIngredient;
    public Transform dishSpawn;
    public GameObject masterPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> potDisplay = new List<GameObject>();
    private Animator animator;
    void Start() {
        droppedIngredient = new List<Ingredient>();
        animator = GetComponent<Animator>();
    }

    public void Cook()
    {
        StartCoroutine(CookCoroutine());
    }

    IEnumerator CookCoroutine() {
        Debug.Log("Cooking");
        droppedIngredient.Sort((a, b) => String.Compare(a.name, b.name));
        animator.SetTrigger("Cook");
        yield return new WaitForSeconds(1f);
       
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
        GameObject item = coll.gameObject;
        if (item.CompareTag("Ingredient") && item.GetComponent<MasterPrefab>().dish is null) {
            droppedIngredient.Add(item.GetComponent<MasterPrefab>().ingredient);
            int index = droppedIngredient.Count - 1;
            spawnPoints[index].GetComponent<Image>().sprite = droppedIngredient[index].sprite;
            spawnPoints[index].SetActive(true);
            Destroy(item);
        }
    }
    
}

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Blackerbox : MonoBehaviour {
    public Cookbook cookbook;
    List<Ingredient> droppedIngredient;
    public Transform dishSpawn;
    public GameObject masterPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    List<GameObject> potDisplay = new List<GameObject>();
    private Animator animator;
    
    #region text_var
    [SerializeField] private Vector2 textoffset;
    private GameObject currentText;
    #endregion

    void Start() {
        droppedIngredient = new List<Ingredient>();
        animator = GetComponent<Animator>();
    }

    public void Cook()
    {
        //Checks if a dish is already waiting to be served
        if (GameObject.FindWithTag("Dish") != null) {
            CreateTextDirectly("Please serve current dish", gameObject.transform, textoffset);
            return;
        }
        
        //Checks if any ingredients in pot
        if (droppedIngredient.Count <= 0) {
            CreateTextDirectly("You need ingredients...", gameObject.transform, textoffset);
            return;
        }
        StartCoroutine(CookCoroutine());
    }

    //Coroutine so that the dish spawns AFTER animation
    IEnumerator CookCoroutine() {
        //Sorts pot
        droppedIngredient.Sort((a, b) => String.Compare(a.name, b.name));
        
        animator.SetTrigger("Cook");
        yield return new WaitForSeconds(1f);

        Cookbook.Recipe poop_recipe = cookbook.recipes[0];
        bool matched = false;

        //Looks through all recipes
        foreach (Cookbook.Recipe recipe in cookbook.recipes) {
            if (recipe.ingredients.Count == droppedIngredient.Count) {
                bool flag = true;
                //If a recipes length matches pot, then checks content
                for (int i = 0; i < recipe.ingredients.Count; i++ ) {
                    if (recipe.ingredients[i].name != droppedIngredient[i].name) {
                        flag = false;
                        break;
                    }
                }
                if (flag) {
                    spawnDish(recipe);
                    matched = true;
                }
            }
        }

        if (!matched)
        {
            spawnDish(poop_recipe);
        }
        
        //Clears visual pot
        droppedIngredient.Clear();
        for (int i = 0; i < spawnPoints.Count; i++) {
            spawnPoints[i].GetComponent<Image>().sprite = null;
            spawnPoints[i].SetActive(false);
        }
        

    }
    private void spawnDish(Cookbook.Recipe recipe)
    {
        GameObject ingredientObj = Instantiate(masterPrefab, dishSpawn.position, dishSpawn.rotation);
        MasterPrefab ingredientItem = ingredientObj.GetComponent<MasterPrefab>();

        ingredientObj.name = recipe.dish.name;
        ingredientItem.sprite = recipe.dish.sprite;
        ingredientItem.name = recipe.dish.name;
        ingredientItem.dish = recipe.dish;

        ingredientItem.ingredientList = new List<Ingredient>(droppedIngredient);
        droppedIngredient.Clear();
        ingredientObj.tag = "Dish";

        CreateTextDirectly("You made " + ingredientItem.name, gameObject.transform, textoffset);
        
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
     private void CreateTextDirectly(string message, Transform parent, Vector2 vector) {
        if (currentText != null) {
            Destroy(currentText);
        
        }
        GameObject go = new GameObject("DynamicText");
        go.transform.SetParent(GameObject.Find("MainCanvas").GetComponent<Canvas>().transform, false);
        go.transform.localPosition = vector;
        currentText= go;
        
        // Add the TMPro component
        TextMeshPro text = go.AddComponent<TextMeshPro>();
        text.text = message;
        text.fontSize = 130;
        text.enableWordWrapping = false;
     }
    
}

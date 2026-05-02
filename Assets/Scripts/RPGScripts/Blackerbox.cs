using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class Blackerbox : MonoBehaviour {
    public Cookbook cookbook;
    List<Ingredient> droppedIngredient;
    public Transform dishSpawn;
    public GameObject masterPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private Sprite[] poopImages;
    List<GameObject> potDisplay = new List<GameObject>();
    private Animator animator;
    
    #region text_var
    [SerializeField] private Vector2 textoffset;
    private GameObject currentText;
    #endregion

    private bool isCooking = false;

    void Start() {
        droppedIngredient = new List<Ingredient>();
        animator = GetComponent<Animator>();
    }

    public void Cook()
    {
        if (isCooking)
        {
            return;
        }
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
        isCooking = true;
        droppedIngredient.Sort((a, b) => String.Compare(a.name, b.name));
        
        animator.SetTrigger("Cook");
        //sfx additoin
        AudioManager.Instance?.PlayFoodCreation();
        
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
                    GameManager.Instance.discoveredDishes.Add(recipe);
                }
            }
        }

        if (!matched)
        {
            //array of items 
            //Sprite selected_poop = poopImages[UnityEngine.Random.Range(0, poopImages.Length)];
            //set image to item from array
            //SpriteRenderer poopRenderer = poop_recipe.dish.GetComponent<SpriteRenderer>();
            //poopRenderer.sprite = selected_poop;
            spawnDish(poop_recipe);
        }
        
        //Clears visual pot
        droppedIngredient.Clear();
        for (int i = 0; i < spawnPoints.Count; i++) {
            spawnPoints[i].GetComponent<Image>().sprite = null;
            spawnPoints[i].SetActive(false);
        isCooking = false;
        }
        

    }
    private void spawnDish(Cookbook.Recipe recipe)
    {
        GameObject ingredientObj = Instantiate(masterPrefab, dishSpawn.position, dishSpawn.rotation);
        MasterPrefab ingredientItem = ingredientObj.GetComponent<MasterPrefab>();


        ingredientObj.name = recipe.dish.name;
        ingredientObj.transform.localScale = new Vector3(.6f, .6f, 1.0f);

        ingredientItem.sprite = recipe.dish.sprite;
        ingredientItem.name = recipe.dish.name;
        ingredientItem.dish = recipe.dish;
        SpriteRenderer sr = ingredientObj.GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Default";
        sr.sortingOrder = -1;

        ingredientItem.ingredientList = new List<Ingredient>(droppedIngredient);
        droppedIngredient.Clear();
        ingredientObj.tag = "Dish";

        CreateTextDirectly("You made " + ingredientItem.name, gameObject.transform, textoffset);
        
    }
    private void OnTriggerEnter2D(Collider2D coll) {
        GameObject item = coll.gameObject;
        if (item.CompareTag("Ingredient") && item.GetComponent<MasterPrefab>().dish is null) {
            //sfx addition
            AudioManager.Instance?.PlayDropFoodIntoPot();

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
        text.fontSize = 120;
        text.enableWordWrapping = false;
     }
    
}

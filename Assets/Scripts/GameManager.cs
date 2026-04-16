using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Queue<Ingredient> inventory;

    public int currRound;
    
    public float roundRemaining;

    #region testing_var
    public Ingredient testIngredient1;
    public Ingredient testIngredient2;
    public Ingredient testIngredient3;
    public Ingredient testIngredient4;
    public Ingredient testIngredient5;
    public Ingredient testIngredient6;
    public Ingredient testIngredient7;
    #endregion

    #region customer_var
    public float[] customerSatisfaction = new float[3];
    public Ingredient[] customerWantedIngredient = new Ingredient[3];
    #endregion

    void Awake() {
        if (Instance == null) {
            Instance = this;
            currRound = 0;
        }
        else if(Instance != this) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        inventory = new Queue<Ingredient>();
        roundRemaining = 10f;
        
        
    }

    public void addInventory(Ingredient ingredient) {
        
        if (inventory.Count >= 9) {
            inventory.Dequeue();
        } 
        inventory.Enqueue(ingredient);  
        
    }
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public Queue<Ingredient> inventory;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if(Instance != this) {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        inventory = new Queue<Ingredient>();
    }

    // Update is called once per frame
    void Update() {
     
    }

    public void addInventory(Ingredient ingredient) {
        Debug.Log("inventory before: " + String.Join(" ", inventory));

        if (inventory.Count >= 9) {
            inventory.Dequeue();
        } 
        inventory.Enqueue(ingredient);  
        Debug.Log("inventory after: " + String.Join(" ", inventory));
    }
}


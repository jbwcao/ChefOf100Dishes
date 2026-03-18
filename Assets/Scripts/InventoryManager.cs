using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    public List<GameObject> spawnPoints = new List<GameObject>();
    public Ingredient[] items = new Ingredient[9];



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This just fills the items index with the inventory from GameManager. Destructive.
        int index = 0;
        Queue<Ingredient> tempInv = GameManager.Instance.inventory;
        while (tempInv.Count > 0)
        {
            Ingredient temp = tempInv.Dequeue();
            items[index] = temp;
            index++;
        }

        // TODO: Instantiate a new GameObject in each slot, maybe needs a prefab for each item? idk how you guys wanna impliment that one.
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

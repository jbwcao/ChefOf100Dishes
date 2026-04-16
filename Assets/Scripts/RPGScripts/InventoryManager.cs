using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

    //FAIR WARNING IM NOT SURE IF THIS WORKS YET SINCE THE QUEUE IS NOT IMPLEMENTED
    
    public List<GameObject> spawnPoints = new List<GameObject>();
    public Ingredient[] items = new Ingredient[9];

    public GameObject masterPrefab;


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

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                GameObject ingredientObj = Instantiate(masterPrefab, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
                MasterPrefab ingredientItem = ingredientObj.GetComponent<MasterPrefab>();
                ingredientObj.tag = "Ingredient";

                // Sets all of the instance variables values of the new gameobject
                ingredientObj.name = items[i].name;
                ingredientItem.inventoryManager = this;
                ingredientItem.sprite = items[i].sprite;
                ingredientItem.name = items[i].name;
                ingredientItem.arrayIndex = i;
                ingredientItem.ingredient = items[i];
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Future button presses will hide unhide the menu
    public void Activate()
    {
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);        
    }

    // When a gameobject is destoryed it clears the corresponding array index
    public void clear(int arrayIndex)
    {
        items[arrayIndex] = null;
    }

    // This is to refill the queue
    public void OnDestroy()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                GameManager.Instance.inventory.Enqueue(items[i]);
            }
        }
    }
}

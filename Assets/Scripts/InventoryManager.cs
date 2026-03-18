using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{

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

                ingredientObj.name = items[i].name;
                ingredientItem.sprite = items[i].sprite;
                ingredientItem.name = items[i].name;
                ingredientItem.arrayIndex = i;
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clear(int arrayIndex)
    {
        items[arrayIndex] = null;
    }
}

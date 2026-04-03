using UnityEngine;
using System.Collections.Generic;


public class Customer : MonoBehaviour
{
    private bool satisfied = false;
    Cookbook cookbook;
    [SerializeField] public List<Dish> wantedDishes;
    public GameObject wantedDishUIPrefab;
    public Vector3 uiOffset = new Vector3(3, 1, 0);


    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject ui = Instantiate(wantedDishUIPrefab, transform.position + uiOffset, Quaternion.identity);
        ui.transform.SetParent(transform);
        ui.GetComponent<WantedDishUI>().SetDishes(wantedDishes);
        //GenerateWantedDish();
        
    }
    /** void GenerateWantedDish()
    {
        int index = Random.Range(0, cookbook.recipes.Count);
        wantedDish = cookbook.recipes[index].dish;

        Debug.Log("Customer wants: " + wantedDish);
    }

    // Update is called once per frame
    */
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        MasterPrefab item = coll.gameObject.GetComponent<MasterPrefab>(); 
        if (wantedDishes.Contains(item.dish))
        {
            wantedDishes.Remove(item.dish);
            Destroy(coll.gameObject);
            
        }
        if (wantedDishes.Count == 0)
        {
            satisfied = true;
            gameObject.SetActive(false);
        }
        Debug.Log(wantedDishes.Count);
        Debug.Log(satisfied);
       
        
    }

        
}

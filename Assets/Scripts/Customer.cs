using UnityEngine;

public class Customer : MonoBehaviour
{
    public Cookbook cookbook;
    public Dish wantedDish;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWantedDish();
        
    }
    void GenerateWantedDish()
    {
        int index = Random.Range(0, cookbook.recipes.Count);
        wantedDish = cookbook.recipes[index].dish;

        Debug.Log("Customer wants: " + wantedDish);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Entered customer trigger: " + coll.gameObject.name);
        MasterPrefab item = coll.gameObject.GetComponent<MasterPrefab>();

        if (item.dish == wantedDish)
        {
            Debug.Log("Customer Satisfied");
            Destroy(coll.gameObject);
        }
    }
}

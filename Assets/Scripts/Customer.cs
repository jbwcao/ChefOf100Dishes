using UnityEngine;

public class Customer : MonoBehaviour
{
    public Cookbook cookbook;
    public string wantedDish;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWantedDish();
        
    }
    string GenerateWantedDish()
    {
        int index = Random.Range(0, cookbook.recipes.Count);
        wantedDish = cookbook.recipes[index].dishname;

        Debug.log("Customer wants: " + wantedDish);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

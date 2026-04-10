using UnityEngine;
using System.Collections.Generic;


public class Customer : MonoBehaviour
{
    private bool satisfied = false;
    public Cookbook cookbook;
    [SerializeField] public List<Ingredient> wantedIngredientList;
    public GameObject wantedIngredientUIPrefab;
    public Vector3 uiOffset = new Vector3(3, 1, 0);
    Ingredient wantedIngredient;
    


    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWantedIngredient();
        
        GameObject ui = Instantiate(wantedIngredientUIPrefab, transform.position + uiOffset, Quaternion.identity);
        ui.transform.SetParent(transform);
        ui.GetComponent<WantedIngredientUI>().SetIngredient(wantedIngredient);
        
        
    }
     void GenerateWantedIngredient()
    {
        int index = Random.Range(0, wantedIngredientList.Count);
        wantedIngredient = wantedIngredientList[index];

        Debug.Log("Customer wants: " + wantedIngredient);
    }

    
    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        MasterPrefab item = coll.gameObject.GetComponent<MasterPrefab>(); 
        foreach (Cookbook.Recipe r in cookbook.recipes) {
            if (r.dish == item.dish)
            {
                foreach (Ingredient i in r.ingredients) {
                    if (wantedIngredient == i)
                    {
                        satisfied=true;
                        Debug.Log("SATISFIED");
                    }
                }
            }
            
        }
       
       
        
    }

        
}

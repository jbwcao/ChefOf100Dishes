using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class Customer : MonoBehaviour
{
    #region satisfaction_var
    [SerializeField] private float maxSatisfied = 10;
    private float currSatisfied = 0;
    private Slider satisfactionSlider;
    #endregion

    #region wanted_ingred_var
    [SerializeField] public List<Ingredient> wantedIngredientList;
    public GameObject wantedIngredientUIPrefab;
    public Vector3 uiOffset = new Vector3(3, 1, 0);
    Ingredient wantedIngredient;
    #endregion

    public Cookbook cookbook;
    


    


    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateWantedIngredient();
        
        GameObject ui = Instantiate(wantedIngredientUIPrefab, transform.position + uiOffset, Quaternion.identity);
        ui.transform.SetParent(transform);
        ui.GetComponent<WantedIngredientUI>().SetIngredient(wantedIngredient);
        
        //slider iniatilize
        satisfactionSlider = GetComponentInChildren<Slider>();
        satisfactionSlider.value = currSatisfied / maxSatisfied;
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
        
        if (item.ingredientList.Contains(wantedIngredient))
        {
            float satisfaction = item.ingredientList.Count; //  satisfaction = complexity
            currSatisfied += satisfaction;

            satisfactionSlider.value = currSatisfied / maxSatisfied;  // update slider

            Destroy(coll.gameObject);  //destroy dish object

            Debug.Log("SATISFIES");

        }

        if (currSatisfied >= maxSatisfied)
        {
            Destroy(gameObject);  //destroy customer once full
        }
       
       
        
    }

        
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


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

            

            Debug.Log("SATISFIES");

        }
        else
        {

            CreateTextDirectly("what is this crap?", gameObject.transform);
        }

        if (currSatisfied >= maxSatisfied)
        {
            Destroy(gameObject);  //destroy customer once full
        }

        Destroy(coll.gameObject);  //destroy dish object
       
    }
    

    public void CreateTextDirectly(string message, Transform parent) {
    GameObject go = new GameObject("DynamicText");
    
    go.transform.SetParent(GameObject.Find("Canvas").transform, false);
    
    // Add the TMPro component
    TextMeshProUGUI text = go.AddComponent<TextMeshProUGUI>();
    text.text = message;
    text.fontSize = 12;
    text.transform.position = new Vector2(-1, 4);
    }


        
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class Customer : MonoBehaviour
{
    #region satisfaction_var
    public int customerIndex;
    [SerializeField] private float maxSatisfied = 10;
    private float currSatisfied;
    private Slider satisfactionSlider;
    #endregion

    #region wanted_ingred_var
    [SerializeField] public List<Ingredient> wantedIngredientList;
    public GameObject wantedIngredientUIPrefab;
    public Vector3 uiOffset = new Vector3(3, 1, 0);
    Ingredient wantedIngredient;
    private GameObject currentIngredientUI;
    #endregion

    #region text_var
    [SerializeField] private Vector2 textOffset = new Vector2(0, 0);
    private GameObject currentText;
    #endregion

    public Cookbook cookbook;
    
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if wanted ingredient is already set from previous round
        if (GameManager.Instance.customerWantedIngredient[customerIndex] != null)
        {
        wantedIngredient = GameManager.Instance.customerWantedIngredient[customerIndex];
        // show existing UI
        GameObject ui = Instantiate(wantedIngredientUIPrefab, transform.position + uiOffset, Quaternion.identity);
        ui.transform.SetParent(transform);
        ui.GetComponent<WantedIngredientUI>().SetIngredient(wantedIngredient);
        currentIngredientUI = ui;
        }
        else
        {
            GenerateWantedIngredient();
        }
        
        currSatisfied = GameManager.Instance.customerSatisfaction[customerIndex];
       
        
        //slider iniatilize
        satisfactionSlider = GetComponentInChildren<Slider>();
        satisfactionSlider.value = currSatisfied / maxSatisfied;

        CreateTextDirectly("I want ...", gameObject.transform, textOffset);
    }
     void GenerateWantedIngredient()
    {
        if (currentIngredientUI != null)
        {
            Destroy(currentIngredientUI);
        } 

        //Choose random from list
        int index = Random.Range(0, wantedIngredientList.Count);
        wantedIngredient = wantedIngredientList[index];

        //save wanted inggredient to game manager
        GameManager.Instance.customerWantedIngredient[customerIndex] = wantedIngredient;


        //Sets visual image
        GameObject ui = Instantiate(wantedIngredientUIPrefab, transform.position + uiOffset, Quaternion.identity);
        ui.transform.SetParent(transform);
        ui.GetComponent<WantedIngredientUI>().SetIngredient(wantedIngredient);
        currentIngredientUI = ui;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        MasterPrefab item = coll.gameObject.GetComponent<MasterPrefab>(); 
        if (coll.gameObject.CompareTag("Ingredient"))
        {
            return;
        }
        

        if (item.ingredientList.Contains(wantedIngredient))
        {
            //satisfaction = complexity
            float satisfaction = item.ingredientList.Count;
            currSatisfied += satisfaction;
            GameManager.Instance.customerSatisfaction[customerIndex] = currSatisfied;

            // update slider
            satisfactionSlider.value = currSatisfied / maxSatisfied;
            GenerateWantedIngredient();

            Debug.Log("SATISFIES");

        }
        else
        {

            CreateTextDirectly("Yuck...", gameObject.transform, textOffset);
            StartCoroutine(Shake(0.3f, 0.1f));
        }

        if (currSatisfied >= maxSatisfied)
        {
            //Destroy customer once full
            Destroy(gameObject);
        }

        //Destroy dish object
        Destroy(coll.gameObject);
       
    }
    

    private void CreateTextDirectly(string message, Transform parent, Vector2 vector) {
        if (currentText != null) {
            Destroy(currentText);
            }
        GameObject go = new GameObject("DynamicText");
        
        go.transform.SetParent(GetComponentInChildren<Canvas>().transform, false);
        go.transform.localPosition = vector;
        currentText= go;
        
        // Add the TMPro component
        TextMeshPro text = go.AddComponent<TextMeshPro>();
        text.text = message;
        text.fontSize = 100;
        text.enableWordWrapping = false;
    
    }
    IEnumerator Shake(float duration, float magnitude) {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration) {
            float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
}
  
}

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    //FAIR WARNING IM NOT SURE IF THIS WORKS YET SINCE THE QUEUE IS NOT IMPLEMENTED
    
    public List<GameObject> spawnPoints = new List<GameObject>();
    public Ingredient[] items = new Ingredient[12];

    public GameObject masterPrefab;
    private Animator animator;
    #region booleans
    private bool redtabOut = false;
    private bool bluetabOut = false;
    private bool recipeOut = false;
    #endregion

    #region co_ords
    [SerializeField] private float tabextendedX;
    [SerializeField] private float tabretractedX;
    
    [SerializeField] private float recipeEnterX;
    [SerializeField] private float recipeExitX;
    [SerializeField] private float recipeEnterY;
    [SerializeField] private float recipeExitY;

    #endregion
   
    #region tabs/buttons
    public GameObject recipePanel;
    public Transform redButton;
    public Transform platformerButton;
    public Transform blueButton;
    public Transform recipeButton;
    public Sprite potSprite;
    public Sprite bookSprite;
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("START Inventory: " + String.Join(", ", GameManager.Instance.inventory));
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
                SpriteRenderer sr = ingredientObj.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "Default";
                sr.sortingOrder = -1;

                ingredientObj.tag = "Ingredient";

                // Sets all of the instance variables values of the new gameobject
                ingredientObj.name = items[i].name;
                ingredientObj.transform.localScale = new Vector3(.8f, .8f, 1.0f);

                
                ingredientItem.inventoryManager = this;
                ingredientItem.sprite = items[i].sprite;
                ingredientItem.name = items[i].name;
                ingredientItem.arrayIndex = i;
                ingredientItem.ingredient = items[i];
            }

        }
        
        Debug.Log("should be none Inventory: " + String.Join(", ", GameManager.Instance.inventory));
        
    }

    public void ToggleRedTab()
    {
        if (bluetabOut)
        {
            ToggleBlueTab();
            return;
        }
        redtabOut = !redtabOut;
        
        animator.SetBool("RedTabOut", redtabOut);
        
        
        redButton.position = redtabOut ? new Vector2(tabextendedX, redButton.position.y) 
                                : new Vector2(tabretractedX, redButton.position.y);
        
        
        platformerButton.gameObject.SetActive(redtabOut);

    }

    public void ToggleBlueTab()
    {
        if (redtabOut)
        {
            ToggleRedTab();
            return;
        }
        bluetabOut = !bluetabOut;
        
        animator.SetBool("BlueTabOut", bluetabOut);
        blueButton.position = bluetabOut ? new Vector2(tabextendedX, blueButton.position.y) 
                                : new Vector2(tabretractedX, blueButton.position.y);
        
        
        recipeButton.gameObject.SetActive(bluetabOut);
        // Accessing the Image component attached to a specific transform
        Image myImage = recipeButton.GetComponent<Image>();
        // Changing the sprite
        myImage.sprite = bookSprite;


        

    }
    public void ToggleRecipeButton()
    {
        recipeOut = !recipeOut;
        recipePanel.SetActive(recipeOut);
        
        recipeButton.position = recipeOut ? new Vector2(recipeExitX, recipeExitY) 
                                : new Vector2(recipeEnterX, recipeEnterY);
        
        Image myImage = recipeButton.GetComponent<Image>();
        // Changing the sprite
        myImage.sprite = bookSprite;
        myImage.sprite = recipeOut ? potSprite 
                                : bookSprite;


        
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
    public void SaveInventory()
    {

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                GameManager.Instance.inventory.Enqueue(items[i]);
            }
        }
        Debug.Log("Inventory: " + String.Join(", ", GameManager.Instance.inventory));
    } 
}

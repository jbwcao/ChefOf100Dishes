using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialRPG : MonoBehaviour
{
    int i = 0;
    [SerializeField] private Vector2 bbtextoffset;
    [SerializeField] private Vector2 inventorytextoffset;
    [SerializeField] private Vector2 customertextoffset;





    [SerializeField]private GameObject inventory;
    [SerializeField]private GameObject blackbox;
    [SerializeField]private GameObject customer;
    [SerializeField]private GameObject button;
    private GameObject currentText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tutorialclick()
    {
        i++;
        if (i == 1)
        {
            inventory.GetComponent<SpriteRenderer>().sortingOrder = 1;
            CreateTextDirectly("This is your inventory.", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 2)
        {
            CreateTextDirectly("Inventory holds 12 ingredients.\nExceed that and the oldest one gets cut.\n\nSuch is life.", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 3)
        {
            CreateTextDirectly("The red tab is for attacking.\n\n(This is a cooking game by the way.)", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 4)
        {
            CreateTextDirectly("The blue tab shows your recipes.\n\nGroundbreaking stuff.", gameObject.transform, inventorytextoffset);

        }
        if (i == 5)
        {
            inventory.GetComponent<SpriteRenderer>().sortingOrder = -4;

          
            blackbox.GetComponent<SpriteRenderer>().sortingOrder = 1;
            CreateTextDirectly("Drag your ingredients here to cook dishes.\nNo pressure, but everyone is hungry.", gameObject.transform, bbtextoffset);

        }
        if (i == 6)
        {

            CreateTextDirectly("The red button is the cook button, press to cook :D", gameObject.transform, bbtextoffset);

        }
        if (i == 7)
        {
            blackbox.GetComponent<SpriteRenderer>().sortingOrder=0;

            
            customer.GetComponent<SpriteRenderer>().sortingOrder = 0;
            CreateTextDirectly("That creature staring at you is a customer.\nFeed it a dish with the ingredient its craving and it'll leave you alone.\nProbably.", gameObject.transform, customertextoffset);


        }
         if (i ==8)
        {

            CreateTextDirectly("Once the yellow bar depletes, it's ggs :(", gameObject.transform, customertextoffset);

        }    
    
        if (i ==9)
        {
            customer.GetComponent<SpriteRenderer>().sortingOrder = -5;
            CreateTextDirectly("Try not to burn the place down...", gameObject.transform, customertextoffset);

            
        }
        if (i == 10)
        {
            CreateTextDirectly(" ", gameObject.transform, customertextoffset);

            button.SetActive(false);
        }

        

    }
    private void CreateTextDirectly(string message, Transform parent, Vector2 vector) {
        if (currentText != null) {
            Destroy(currentText);
        
        }
        GameObject go = new GameObject("DynamicText");
        go.transform.SetParent(GameObject.Find("MainCanvas").GetComponent<Canvas>().transform, false);
        go.transform.localPosition = vector;
        go.layer = LayerMask.NameToLayer("UI");
        currentText= go;
        
        // Add the TMPro component
        TextMeshPro text = go.AddComponent<TextMeshPro>();
        text.text = message;
        text.fontSize = 120;
        text.enableWordWrapping = false;
        text.sortingOrder = 10;
        text.alignment = TextAlignmentOptions.Center;
     }
}

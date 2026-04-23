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
            CreateTextDirectly("This is your inventory!", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 2)
        {
            CreateTextDirectly("Once you have more than 12 ingredients, the first will be destroyed", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 3)
        {
            CreateTextDirectly("Press the red tab to switch to attack", gameObject.transform, inventorytextoffset);
            
        }
        if (i == 4)
        {
            CreateTextDirectly("Press the blue tab to view recipes", gameObject.transform, inventorytextoffset);

        }
        if (i == 5)
        {
            inventory.GetComponent<SpriteRenderer>().sortingOrder = -4;

          
            blackbox.GetComponent<SpriteRenderer>().sortingOrder = 1;
            CreateTextDirectly("Drag Ingredients here to cook dishes ", gameObject.transform, bbtextoffset);

        }
        if (i == 6)
        {
            blackbox.GetComponent<SpriteRenderer>().sortingOrder=0;

            
            customer.GetComponent<SpriteRenderer>().sortingOrder = 0;
            CreateTextDirectly("This is a customer, feed it dishes with the ingredient it wants to satisfy it! ", gameObject.transform, customertextoffset);


        }
        if (i ==7)
        {
            customer.GetComponent<SpriteRenderer>().sortingOrder = -2;
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
     }
}

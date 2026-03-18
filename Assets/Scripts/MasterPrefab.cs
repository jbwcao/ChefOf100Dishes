using UnityEngine;

public class MasterPrefab : MonoBehaviour
{

    public int arrayIndex;
    public Sprite sprite;

    public string name;
    public InventoryManager inventoryManager;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = FindFirstObjectByType<InventoryManager>();
        GetComponent<SpriteRenderer>().sprite = this.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy()
    {
        if (inventoryManager != null)
        {
           inventoryManager.clear(arrayIndex);
        }

    }
}

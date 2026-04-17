using UnityEngine;
using UnityEngine.SceneManagement;

public class RpgToPlatform : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void Clicked()
    {
        GameManager.Instance.roundRemaining--;
        FindObjectOfType<InventoryManager>().SaveInventory();

        if (GameManager.Instance.roundRemaining <= 0)
        {
         // game over
        }
        SceneManager.LoadScene("Hub Room");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

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
        Debug.Log(GameManager.Instance.roundRemaining);

        if (GameManager.Instance.roundRemaining < 0)
        {
            SceneManager.LoadScene("Game Over");
            return;
        }
        SceneManager.LoadScene("Hub Room");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

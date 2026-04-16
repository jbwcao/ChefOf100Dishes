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
        SceneManager.LoadScene("Hub Room");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

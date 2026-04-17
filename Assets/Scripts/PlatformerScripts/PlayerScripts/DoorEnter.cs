using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorEnter : MonoBehaviour
{
    private bool enterFlag;
    private string sceneName;

    private void Start()
    {
        enterFlag = false;
    }

    private void Update()
    {
        if(enterFlag && Keyboard.current.eKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") ) 
        {
            enterFlag = true;
            sceneName = collision.gameObject.GetComponent<DoorScript>().sceneName;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
        {
            enterFlag = false;
        }
    }
}

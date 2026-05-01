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
            //sfx additon
            AudioManager.Instance?.PlayEnterPortal();
            
            SceneManager.LoadScene(sceneName);
        }    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Door")) 
        {
            // spaghetti way of just checking one of two scripts 
            enterFlag = true;
            sceneName = collision.gameObject.GetComponent<DoorWithSignScript>().sceneName;
            if (sceneName == "")
            {
                Debug.Log("got here");
                sceneName = collision.gameObject.GetComponent<DoorScript>().sceneName;
            }
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

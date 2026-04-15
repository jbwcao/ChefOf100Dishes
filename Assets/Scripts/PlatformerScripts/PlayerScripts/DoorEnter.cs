using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorEnter : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Door") && Keyboard.current.eKey.wasPressedThisFrame) 
        {
            SceneManager.LoadScene(collision.gameObject.GetComponent<DoorScript>().sceneName);
        }
    }
}

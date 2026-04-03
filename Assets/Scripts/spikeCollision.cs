using UnityEngine;

public class spikeCollision : MonoBehaviour
{

    public int contactDamage = 1;

    public Rigidbody2D RB;
    public Collider2D Collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            //contact damage returns true if i-frames are off, otherwise skips knockback call 
            if (collision.gameObject.GetComponent<PlayerHealth>().takeDamage(contactDamage))// get the damage/health script from player and call takedamage()
            {
                collision.gameObject.GetComponent<PlayerMovement>().applyKnockback(transform.position);// can add enemy specific knockback if needed 
            } 
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            //contact damage returns true if i-frames are off, otherwise skips knockback call 
            if (collision.gameObject.GetComponent<PlayerHealth>().takeDamage(contactDamage))// get the damage/health script from player and call takedamage()
            {
                collision.gameObject.GetComponent<PlayerMovement>().applyKnockback(transform.position);// can add enemy specific knockback if needed 
            } 
        }
    }
}

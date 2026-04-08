using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyHitbox : MonoBehaviour, IDamageable
{

    public int maxHealth;
    int currHealth;

    public int contactDamage;

    public Rigidbody2D EnemyRB;
    public Collider2D EnemyCollider;

    public GameObject[] droppedItems;

    void Start()
    {
         currHealth = maxHealth;
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }


//isn't used, check EnemyMovement for used version, make this usable 
    public void applyKnockback(Vector2 hitFromPosition, float upwardForce = 0f, float knockbackForce = 8f)
    {

        float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

        //set x velocity to 0 for smoother knockback
        EnemyRB.linearVelocityX = 0f;

        //direction is angled a little bit upward for pazzaz
        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;


        //force applied to enemy
        EnemyRB.AddForce(force, ForceMode2D.Impulse);
        
    }

    public void takeDamage(int damage)
    {
        currHealth -= damage;
        Debug.Log(name + " took damage, health left: " + currHealth);
        //coroutine flash white in sprite renderer
        if (currHealth <= 0)
        {
            death();
        }
    }

    private void death()
    {
        //drop a designated food item
        foreach (GameObject i in droppedItems)
        {
            if (i != null)
            {
                Instantiate(i, transform.position, transform.rotation);
            }
            
        }
        Destroy(this.gameObject); //May want to update to drop a corpse on death as well + some effects
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




}

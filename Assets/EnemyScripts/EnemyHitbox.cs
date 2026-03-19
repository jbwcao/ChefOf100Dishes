using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyHitbox : MonoBehaviour
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

    void applyKnockback()
    {
        // get swing direction
        // get enemy direction
        // move enemy away from player attack
    }

    public void takeDamage(int damage)
    {
        currHealth -= damage;
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
            Instantiate(i, transform.position, transform.rotation);
            
        }
        Destroy(this.gameObject); //May want to update to drop a corpse on death as well + some effects
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(contactDamage); // get the damage/health script from player and call takedamage()
        }
    }




}

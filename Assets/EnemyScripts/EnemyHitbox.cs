using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{

    public int maxHealth;
    int currHealth;

    public int contactDamage;

    Rigidbody2D EnemyRB;

    void Start()
    {
         currHealth = maxHealth;
         EnemyRB = GetComponent<Rigidbody2D>();
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

    void takeDamage(int damage)
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
        Destroy(this.gameObject); //May want to update to drop a corpse on death as well + some effects
    }




     private void OnCollisionEnter2D(Collision2D other)
    {
       if (other.transform.CompareTag("Player"))
        {
            //other.gameObject.GetComponent<> // get the damage/health script from player and call takedamage()
        }
    }




}

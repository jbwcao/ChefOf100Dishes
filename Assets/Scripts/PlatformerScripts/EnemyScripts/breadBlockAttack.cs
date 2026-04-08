using UnityEngine;

public class breadBlockAttack : EnemyMovement
{
    public LayerMask playerLayers;
    public float lungePower = 2f;
    public int swingDamage = 1;
    public float timeBeforecountering = 0.15f;
    public Vector2 attackSize = new Vector2(1f, 0.5f);
    public float range = 1.15f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // if the player attacks facing the front of enemy:
    // less knockback
    public override void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2, float knockbackForce = 8)
    {
        if (hitFromPosition.x > transform.position.x) // if player attacks from the front(Implement)
        {
            // play a block animation
            base.applyKnockback(hitFromPosition, 0, 2);
            //attack after -timeBeforecountering- seconds
        }
        else
        {
            base.applyKnockback(hitFromPosition, upwardForce, knockbackForce);
        }
        
    }

    
    
    // deal no damage
    

    //bread then counters with a swing attack, swing moves him forward a little

    void attack(float attackDirectionOffset)
    {
        Vector2 attackCenter = (Vector2) transform.position + new Vector2(attackDirectionOffset, 0);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, playerLayers);
        //play swing attack animation here
        foreach(Collider2D target in enemiesHit)
        {
            if (target.CompareTag("player"))
            {
                if (target.gameObject.GetComponent<PlayerHealth>().takeDamage(swingDamage))// get the damage/health script from player and call takedamage()
                {
                    target.gameObject.GetComponent<PlayerMovement>().applyKnockback(transform.position);// can add enemy specific knockback if needed 
                } 
            }
        }
    }


}

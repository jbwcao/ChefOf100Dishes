using System.Collections;
using UnityEngine;

public class breadBlockAttack : EnemyMovement, IKnockbackable
{
    public LayerMask playerLayers;
    public float lungePower = 2f;
    public int swingDamage = 1;
    public float timeBeforecountering = 0.15f;
    public Vector2 attackSize = new Vector2(1f, 0.5f);
    public float range = 1.15f;
    private bool stop = false;
    private Animator animator;
    public Animator attackAnimator;

    public Transform slashPos;
    private Vector3 originalLocalPos;
    public SpriteRenderer slashSR;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    { 
        animator = GetComponent<Animator>();
        originalLocalPos = slashPos.localPosition;
        
            base.Start();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(!stop){
            base.Update();

            slashSR.flipX = currentDir > 0;
            SetFacing(currentDir > 0);
        }
    }

    // if the player attacks facing the front of enemy:
    // less knockback
    public override bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2, float knockbackForce = 8)
    {
        float horizontalDifference = hitFromPosition.x - transform.position.x;
        bool hitFromFront = (horizontalDifference * currentDir) > 0;
        if (hitFromFront) // if player attacks from the front(Implement)
        {
            // play a block animation
            animator.Play("Block", 0, 0f);
            base.applyKnockback(hitFromPosition, 0, 2);
            StartCoroutine(Counter());
            //attack after -timeBeforecountering- seconds
            return false;
        }
        else
        {
            return base.applyKnockback(hitFromPosition, upwardForce, knockbackForce);
        }
    }

    
    
    // deal no damage
    

    IEnumerator Counter()
    {
        //stop movement speed
        stop = true;
        //animator.Play("Attack", 0, 0f);
        attackAnimator.Play("Breadflare", 0, 0f);
        yield return new WaitForSeconds(timeBeforecountering);
        attack(currentDir * range);
    
    }


    void attack(float attackDirectionOffset)
    {
        //bread then counters with a swing attack, swing moves him forward a little
        //animator.Play("Attack", 0, 0f);
        Vector2 attackCenter = (Vector2) transform.position + new Vector2(attackDirectionOffset, 0);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, playerLayers);
        //play swing attack animation here
        foreach(Collider2D target in enemiesHit)
        {
            if (target.CompareTag("Player"))
            {
                if (target.gameObject.GetComponent<PlayerHealth>().takeDamage(swingDamage))// get the damage/health script from player and call takedamage()
                {
                    target.gameObject.GetComponent<PlayerMovement>().applyKnockback(transform.position);// can add enemy specific knockback if needed 
                } 
            }
        }
        //start movement
        stop = false;
    }

    public void SetFacing(bool facingRight)
    {
        Vector3 pos = originalLocalPos;
        pos.x = facingRight ? Mathf.Abs(originalLocalPos.x) : -Mathf.Abs(originalLocalPos.x);
        slashPos.localPosition = pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + new Vector2(range, 0), attackSize);
    }
}

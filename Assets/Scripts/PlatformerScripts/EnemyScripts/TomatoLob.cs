using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class TomatoLob : MonoBehaviour
{
    

    // the type of projectile the tomato will toss
    public GameObject projectile;
    
    [Header("Firing stats")]
    public int projectileAmount = 1;
     public float fireRate = 3f;
    public float projectileBurstIntervals = 0.05f;
   
    private float attackCooldown;
    public float spread = 0f;

    //time to reach dest determines the arc at which the bomb is thrown
    [Header("Projectile arc")]
    public float minTimeToReachDest = 1.5f;
    public float MaxTimeToReachDest = 2.5f;



    //shoot out a raycast in all directions, if this hit player start attacking
    //get player current location and save the point
    //calculate the trajectory of the projectile and shoot
    //begin cooldown until next shot

    [Header("Player Search Var")]
    public float detectionRadius = 12f;

    //private bool playerDetected = false; 
    private bool attacking = false;
    private int searchMask; //what it checks for 
    public LayerMask blockMask; // what blocks view


    SpriteRenderer sr;
    public Rigidbody2D rb;
    public Animator animator;
    public String attackAnimation;
    protected virtual void Start()
    {
        //BUG: currently can detect the players attack as part of their body, put on different layer?
        sr = GetComponent<SpriteRenderer>();
        searchMask = LayerMask.GetMask("Player");
        attackCooldown = fireRate; 

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //should only look around once 
        if (!attacking)
        {
            //need to stop checking every frame
            lookAround();
        }
    

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }


    protected virtual void lookAround()
    {
        //get all objects in view radius
        Collider2D[] viewArea = Physics2D.OverlapCircleAll(transform.position, detectionRadius, searchMask);
        foreach (Collider2D target in viewArea)
        {
            if (target.CompareTag("Player"))
            {
                //draw raycast, checks for any terrain between enemy and player
                RaycastHit2D blockHit = Physics2D.Linecast(transform.position, target.transform.position, blockMask);

                if (blockHit.collider != null)
                {
                    // something in terrain blocked the explosion
                    continue;
                }

                if (attackCooldown <= 0)
                {   
                    if(transform.position.x < target.transform.position.x){ //player is to the right
                            sr.flipX = true;
                    }
                    else
                    {
                        sr.flipX = false;
                    }

                    
                    
                    
                    StartCoroutine(Attack(target.transform.position));
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }


    private void alert()
    {
        // act like a little notice startle
        //playerDetected = true;
        rb.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);;
    }
 

    protected virtual IEnumerator Attack(Vector3 attackLocation)
    {
        Debug.Log("Starting attack Chain");
        attacking = true;

        for(int i=0; i < projectileAmount; i++)
        {

            //random time to reach locataion
            float timeToReachDest = UnityEngine.Random.Range(minTimeToReachDest, MaxTimeToReachDest);

            Vector2 start = transform.position;
            Vector2 end = (Vector2)attackLocation + new Vector2(UnityEngine.Random.Range(-spread, spread), 0);

            float vx = (end.x - start.x) / timeToReachDest;
            float vy = (end.y - start.y - 0.5f * Physics2D.gravity.y * timeToReachDest) / timeToReachDest;
            //0.5f represents the arc, up to 1.5f for large arcs


            //attack animation, cause this gets used multiple times assign attack and animator manuly 
                if (animator != null)
                {
                    animator.Play(attackAnimation, 0, 0f);
                }




            //TODO - instead of instantation and destroying objects, pool items
            //create and launch the projectile
            GameObject tomato = Instantiate(projectile, transform.position, transform.rotation);
            Rigidbody2D rb = tomato.GetComponent<Rigidbody2D>();
            rb.linearVelocity = new Vector2 (vx, vy);

            //wait  projectileBurstInterval before fireing again
             yield return new WaitForSeconds(projectileBurstIntervals);



  
        }

        attackCooldown = fireRate; 
        attacking = false;
    }

    internal void Attack()
    {
        throw new NotImplementedException();
    }
}

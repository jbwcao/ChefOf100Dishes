using System;
using System.Collections;
using UnityEngine;
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
    public int rayCount = 36; // amount of angles it checks around itself
    public float raydistance = 15f; //player detection radius

    private bool playerDetected = false;
    private bool attacking = false;
    private int visionMask; //what it checks for 


    public Rigidbody2D rb;
    void Start()
    {
        //BUG: currently can detect the players attack as part of their body, put on different layer?
        visionMask = LayerMask.GetMask("Player", "Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        //should only look around once 
        if (!attacking)
        {
            lookAround();
        }
    

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }


    private void lookAround()
    {
        //logic for player detection, shoots a ray in every direction
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * (360f / rayCount);
            float radians = angle * Mathf.Deg2Rad;

            Vector2 direction = new Vector2 (Mathf.Cos(radians), Mathf.Sin(radians));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raydistance, visionMask);

            //show notice to the player position 

            if(hit.collider != null && hit.collider.CompareTag("Player")) // add another statment checking cooldown
            {
                if(attackCooldown <= 0)
                {
                    
                    //attack(hit.collider.transform.position);
                    StartCoroutine(Attack(hit.collider.transform.position));
                    break;
                }


                /*
                if(!playerDetected)
                {
                    //start grace timer
                    alert();
                }

                if( graceTimer <= 0 && attackCooldown <= 0)
                {
                    
                    attack(hit.collider.transform.position);
                }
                */
                // need a way to keep track of player if player is not in sight

            }


            //Following code is used to visulaize raycasts
           if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, (Vector2)transform.position + direction * raydistance, Color.red);
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.DrawRay(transform.position, (Vector2)transform.position + direction * raydistance, Color.green);
            }
        }
    }


    private void alert()
    {
        // act like a little notice startle
        playerDetected = true;
        rb.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);;
    }
 

    private IEnumerator Attack(Vector3 attackLocation)
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

}

using System;
using UnityEngine;

public class TomatoLob : MonoBehaviour
{
    

    // the type of projectile the tomato will toss
    public GameObject projectile;
    public int projectileAmount = 1;
    public float projectileBurstIntervals = 0.05f;
    public float fireRate = 3f;
    private float attackCooldown;
    public float spread = 0f;

    private bool playerDetected = false;
    public float graceTimeBeforeAttack = 1.5f;
    private float graceTimer;

    private int visionMask; //what it checks for 

    //shoot out a raycast in all directions, if this hit player start attacking
    //get player current location and save the point
    //calculate the trajectory of the projectile and shoot
    //begin cooldown until next shot

    public int rayCount = 36; // amount of angles it checks around itself
    public float raydistance = 15f; //player detection radius

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
        lookAround();

        if (graceTimer > 0)
        {
            graceTimer -= Time.deltaTime;
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

            //show notice to the player position and wait for a grace time 

            //start grace timer while keeping track of player
            if(hit.collider != null && hit.collider.CompareTag("Player")) // add another statment checking cooldown
            {
                if(attackCooldown <= 0)
                {
                    
                    
                    attack(hit.collider.transform.position);
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
    private void detectionVisualizer(RaycastHit2D hit, Vector2 direction)
    {
        if (hit.collider != null)
            {
                Debug.DrawRay(transform.position, hit.point, Color.red);
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.DrawRay(transform.position, (Vector2)transform.position + direction * raydistance, Color.green);
            }
    }


    private void alert()
    {
        // act like a little notice startle
        graceTimer = graceTimeBeforeAttack;
        playerDetected = true;
        rb.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);;
    }

    //may switch to a coroutine 
    private void attack(Vector3 attackLocation)
    {
        
        //attack logic 
        for(int i=0; i < projectileAmount; i++)
        {
            //calculate arc between player position and current position 
            //calculate for spread
            //wait  projectileBurstInterval before fireing again

            //placeholder, teleports bomb to player location
            Instantiate(projectile, attackLocation, transform.rotation);
        }

        attackCooldown = fireRate;
    }
}

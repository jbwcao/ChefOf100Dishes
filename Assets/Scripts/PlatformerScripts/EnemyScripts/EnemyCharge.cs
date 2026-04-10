using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyCharge : EnemyMovement
{

    //TOFIX enrage area activates knockback
    //Accelerate speed
    // Add a disengage trigger
    // an alert queue for when enemy starts chasing 
    public Vector2 agrroRange = new Vector2(15f, 10f);
    public Vector2 deagrroRange = new Vector2(15f, 20f);
    
    
    public float agroSpeed = 2f;
    public float searchIntervals = 0.25f;

    //public float knockbackTime = 0.15f;
    //private bool stopMoving = false;



    private bool isChasing = false;
    public EnemyMovement idleScript; 
    public LayerMask playerLayer;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SearchForPlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isChasing && player != null)
        {
            float distance = Mathf.Abs(rb.position.x - player.position.x);
        
            if (distance <= 0.01) return;

            Vector2 nextPosition;
            

            nextPosition.x = (player.position.x > rb.position.x ? 1f : -1f) * agroSpeed;
            sr.flipX = player.position.x > rb.position.x ? true : false;

            nextPosition.y = rb.linearVelocityY;
            rb.linearVelocity = nextPosition;

           // Vector2 nextPosition = Vector2.MoveTowards(
           // rb.position, new Vector2(player.position.x, 0), agroSpeed * Time.fixedDeltaTime
            //);

            //rb.MovePosition(nextPosition);
        }
    }

    IEnumerator SearchForPlayer()
    {
        while(true){
            //check to see if player is in range and to agro
            while (!isChasing)
            {
                yield return new WaitForSeconds(searchIntervals);


                Collider2D[] viewArea = Physics2D.OverlapBoxAll(transform.position, agrroRange, 0f, playerLayer);
                foreach (Collider2D hit in viewArea) {
                    if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Player Found!");
                        //TODO add a alert animation + sfx
                        player = hit.transform;
                        isChasing = true;
                        //disable idle
                        idleScript.enabled = false;
                    }
                }
            }


            //check if needed to de-argro
            while (isChasing)
            {
                Collider2D[] viewArea = Physics2D.OverlapBoxAll(transform.position, deagrroRange, 0f, playerLayer);
                //bool playerGone = true;
                //player is not in view 
                if (viewArea.Length == 0)
                {
                    player = null;
                    isChasing = false;

                    //if knockback is happening
                    //CancelKnockback();

                    idleScript.enabled = true;
                }

            
                yield return new WaitForSeconds(searchIntervals);
            }

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, agrroRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, deagrroRange);

    }




   /* public virtual void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
    {
        CancelInvoke(nameof(EndKnockback));

        stopMoving = true;

         float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

        //set x velocity to 0 for smoother knockback
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);

        //switch to end knockback when landing instead of timer(?)
        Invoke(nameof(EndKnockback), knockbackTime);
    }

    public void CancelKnockback()
{
    CancelInvoke(nameof(EndKnockback));
    stopMoving = false;
    rb.linearVelocity = Vector2.zero;
}

    void EndKnockback()
    {
        stopMoving = false;
    }*/
}


using UnityEngine;
using System.Collections;

public class EnemyCharge : EnemyMovement
{

    //TOFIX enrage area activates knockback
    //Accelerate speed
    // Add a disengage trigger
    // an alert queue for when enemy starts chasing 
    [SerializeField] private EnemyMovement basicMovement;
    private bool isChasing;
    
    public bool horizontalOnly = true;
    public float chaseSpeed = 3f;
    private float currentSpeed = 1f;
    private Transform target;
    public BoxCollider2D trigger;
    protected Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        basicMovement = GetComponent<EnemyMovement>();
        rb = GetComponent<Rigidbody2D>();
        isChasing = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isChasing || target == null)
            return;

        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        if (horizontalOnly)
        {
            direction = new Vector2(Mathf.Sign(direction.x), 0f);
        }

        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);
    }

    //If player is 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasing = true;
            basicMovement.enabled = false;
            target = collision.transform;
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = null;
            isChasing = false;
            basicMovement.enabled = true;
        }
        
    }


    private IEnumerator Accelerate()
    {
        
        yield return null;
    }

}

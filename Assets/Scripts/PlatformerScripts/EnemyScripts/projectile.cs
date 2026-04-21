using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class projectile : MonoBehaviour
{
    public int set_rotation = 0;
    public float projectileSpeed = 0.5f;
    public int damage = 1;


    public float bulletLifespan = 5f;
    private float lifespanTimer;


    public bool tracking = false;
    public float trackingStrength = 0.5f;
    private Vector2 movement;
    

    public LayerMask blockingLayer;
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 fixedTarget;
    private Vector2 direction;
    void Start()
    {
       lifespanTimer = bulletLifespan;
       rb = GetComponent<Rigidbody2D>();
       transform.eulerAngles = new Vector3(0, 0, set_rotation);
       player = GameObject.Find("Player").transform;

       fixedTarget = player.transform.position;
       direction = (fixedTarget - (Vector2)transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
        movement =  direction * projectileSpeed;
        rb.linearVelocity = movement;


        lifespanTimer -= Time.deltaTime;
        if (lifespanTimer <= 0)
        {
            bulletbreak();
        }
    }

    //replace this with bullet destroy visuals
    void bulletbreak()
    {
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().takeDamage(damage);
            collision.GetComponent<PlayerMovement>().applyKnockback(transform.position);
            bulletbreak();
        }
        
        if (((1 << collision.gameObject.layer) & blockingLayer) != 0)
        {
            bulletbreak();
        }
    }
}

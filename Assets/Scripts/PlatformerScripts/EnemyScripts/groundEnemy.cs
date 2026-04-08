using UnityEngine;

public class groundEnemy : MonoBehaviour
{
    [Header("Movement Variables")]
    public float enemySpeed = 1f;
    public int direction = 1; //can be 1 or -1
    int currentDir;

    float halfwidth;// sprite width
    float halfhight;

    private bool stopMoving = false;
    public float knockbackTime = 0.15f;

    Vector2 movement;

    [Header("Hitbox Variables")]
    public int maxHealth;
    int currHealth;

    public int contactDamage;

    public GameObject[] droppedItems;
    public Collider2D EnemyCollider;

    Rigidbody2D EnemyRB;
    
    SpriteRenderer sprite;

    // We can query the bitmask once rather than for every attack
    int terrainLayer;

    // TODO: implent a stop before turning and moving again
    void Start()
    {
        //Movement
        terrainLayer = LayerMask.GetMask("Terrain");
        EnemyRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentDir = direction;
        halfwidth = sprite.bounds.extents.x;
        halfhight = sprite.bounds.extents.y;

        //Health
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving)
        {
            directionChecker();
            basic_move();  
        }

        
    }

    #region Movement

    private void basic_move(){
        movement.x = enemySpeed * currentDir;
        movement.y = EnemyRB.linearVelocityY;
        EnemyRB.linearVelocity = movement;
        
    }


    //TODO: Enemies freak out when airborn, fix
    private void directionChecker()
    {
        Vector2 rightPos = transform.position;
        Vector2 leftPos = transform.position;
        rightPos.x += halfwidth;
        leftPos.x -= halfwidth;

        if(EnemyRB.linearVelocityX > 0)
        {
            //draw a ray that points right for checking for walls and check if enemy is moving right 
            //second statment checks if there's no floor
            if (Physics2D.Raycast(transform.position, Vector2.right, halfwidth + 0.1f, terrainLayer) ||
            !Physics2D.Raycast(rightPos, Vector2.down , halfwidth + 0.1f, terrainLayer))
            {
                currentDir *= -1;
                sprite.flipX = true;

            
            }
        } else if(EnemyRB.linearVelocityX < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfwidth + 0.1f, terrainLayer) ||
             !Physics2D.Raycast(leftPos, Vector2.down , halfwidth + 0.1f, terrainLayer))
            {
                currentDir *= -1;
                sprite.flipX = false;
            }
        }
    }

    //virtual allows for overriding in subclasses
    public virtual void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
    {
        stopMoving = true;

        float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

        //set x velocity to 0 for smoother knockback
        EnemyRB.linearVelocityX = 0f;

        //direction is angled a little bit upward for pazzaz
        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;


        //force applied to enemy
        EnemyRB.AddForce(force, ForceMode2D.Impulse);

        //switch to end knockback when landing instead of timer(?)
        Invoke(nameof(EndKnockback), knockbackTime);
    }

    
    void EndKnockback()
    {
        stopMoving = false;
    }

    #endregion Movement
    #region Health

    public void takeDamage(int damage)
    {
        currHealth -= damage;
        Debug.Log(name + " took damage, health left: " + currHealth);
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
            if (i != null)
            {
                Instantiate(i, transform.position, transform.rotation);
            }
            
        }
        Destroy(this.gameObject); //May want to update to drop a corpse on death as well + some effects
    }



     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            //contact damage returns true if i-frames are off, otherwise skips knockback call 
            if (collision.gameObject.GetComponent<PlayerHealth>().takeDamage(contactDamage))// get the damage/health script from player and call takedamage()
            {
                collision.gameObject.GetComponent<PlayerMovement>().applyKnockback(transform.position);// can add enemy specific knockback if needed 
            } 
            
        }
    }

}
#endregion Health
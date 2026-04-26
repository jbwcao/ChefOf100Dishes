using NUnit.Framework.Internal;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IKnockbackable
{
    public float enemySpeed = 1f;
    public int direction = 1; //can be 1 or -1
    protected int currentDir;

    float halfwidth;// sprite width
    float halfhight;

    private bool stopMoving = false;
    public float knockbackTime = 0.15f;

    public Animator anim;
    public string hurtName;
    Vector2 movement;

    Rigidbody2D EnemyRB;
    Collider2D col;
    SpriteRenderer sprite;

    // We can query the bitmask once rather than for every attack
    int terrainLayer;

    // TODO: implent a stop before turning and moving again
    private void OnEnable()
    {
        if(currentDir != 0)
            //double check enemy is facing the right direction when renableing script(may be wrong)
            if (currentDir > 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;   
            }
    }



    protected virtual void Start()
    {
        terrainLayer = LayerMask.GetMask("Terrain");
        EnemyRB = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentDir = direction;
        halfwidth = sprite.bounds.extents.x;
        halfhight = sprite.bounds.extents.y;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!stopMoving)
        {

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, col.bounds.min.y - 0.01f), new Vector2(0, -1), 0.1f, terrainLayer);
            bool onGround = hit.collider == null? false : hit.collider.CompareTag("Platform");
            if (onGround)
            {
                directionChecker();
            }  
            basic_move();  
        }
        else
        {
            if(anim != null && hurtName != null)
            {
                //this is meant to play a hurt animation while the enemy is getting knocked back, doesn't work though
                anim.Play(hurtName, 0, 0f);
            }
        }
        
    }


    public virtual void basic_move(){
        movement.x = enemySpeed * currentDir;
        movement.y = EnemyRB.linearVelocityY;
        EnemyRB.linearVelocity = movement;
        
    }


    //TODO: Enemies freak out when airborn, fix
    public virtual void directionChecker()
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
                sprite.flipX = false;

            
            }
        } else if(EnemyRB.linearVelocityX < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfwidth + 0.1f, terrainLayer) ||
             !Physics2D.Raycast(leftPos, Vector2.down , halfwidth + 0.1f, terrainLayer))
            {
                currentDir *= -1;
                sprite.flipX = true;
            }
        }
    }

    //vertual allows for overriding in subclasses
    public virtual bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
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
        return true;
    }




    
    void EndKnockback()
    {
        stopMoving = false;
    }
}

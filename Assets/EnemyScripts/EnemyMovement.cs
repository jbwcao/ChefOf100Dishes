using NUnit.Framework.Internal;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float enemySpeed = 1f;
    public int direction = 1; //can be 1 or -1
    int currentDir;

    float halfwidth;// sprite width
    float halfhight;

    private bool stopMoving = false;
    public float knockbackTime = 0.15f;

    Vector2 movement;

    Rigidbody2D EnemyRB;
    SpriteRenderer sprite;

    // We can query the bitmask once rather than for every attack
    int terrainLayer;

    // TODO: implent a stop before turning and moving again
    void Start()
    {
        terrainLayer = LayerMask.GetMask("Terrain");
        EnemyRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentDir = direction;
        halfwidth = sprite.bounds.extents.x;
        halfhight = sprite.bounds.extents.y;
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
            
            }
        } else if(EnemyRB.linearVelocityX < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfwidth + 0.1f, terrainLayer) ||
             !Physics2D.Raycast(leftPos, Vector2.down , halfwidth + 0.1f, terrainLayer))
            {
                currentDir *= -1;
            }
        }
    }


    public void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
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
}

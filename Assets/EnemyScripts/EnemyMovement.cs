using NUnit.Framework.Internal;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float enemySpeed = 1f;
    public int direction = 1; //can be 1 or -1
    int currentDir;

    float halfwidth;// sprite width
    float halfhight;

    Vector2 movement;

    Rigidbody2D EnemyRB;
    SpriteRenderer sprite;

    // TODO: implent a stop before turning and moving again
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        currentDir = direction;
        halfwidth = sprite.bounds.extents.x;
        halfhight = sprite.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        directionChecker();
        basic_move();
    }


    private void basic_move(){
        movement.x = enemySpeed * currentDir;
        movement.y = EnemyRB.linearVelocityY;
        EnemyRB.linearVelocity = movement;
        
    }


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
            if (Physics2D.Raycast(transform.position, Vector2.right, halfwidth + 0.1f, LayerMask.GetMask("Terrain")) ||
            !Physics2D.Raycast(rightPos, Vector2.down , halfwidth + 0.1f, LayerMask.GetMask("Terrain")))
            {
                currentDir *= -1;
            
            }
        } else if(EnemyRB.linearVelocityX < 0)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, halfwidth + 0.1f, LayerMask.GetMask("Terrain")) ||
             !Physics2D.Raycast(leftPos, Vector2.down , halfwidth + 0.1f, LayerMask.GetMask("Terrain")))
            {
                currentDir *= -1;
            }
        }


        
    }

}

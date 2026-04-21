using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class SlimeMove : MonoBehaviour, IKnockbackable
{
    //Jumps towards the player, Ignores ledges
    //drops a smaller slime on death
    public float jumpDownTime = 1.5f;
    public float jumpVarience = 0.15f;
    public float jumpPowerX = 5f;
    public float jumpPowerY = 3.5f;


    public float squashTime = 0.1f;
    public float stretchTime = 0.08f;
    public Vector3 squashScale = new Vector3(1.2f, 0.8f, 1f);
    public Vector3 stretchScale = new Vector3(0.8f, 1.2f, 1f);

    private Vector3 originalScale;


    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()    
    {
        player =  GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
    }


    IEnumerator Jump()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpDownTime + Random.Range(-jumpVarience, jumpVarience));
            yield return JumpStretch();
            
            float xDir = player.position.x > transform.position.x ? 1f : -1f;
            sr.flipX = player.position.x > transform.position.x ? false : true;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            rb.AddForce(new Vector2(xDir * jumpPowerX, jumpPowerY), ForceMode2D.Impulse);    
        }
        
    }

    private IEnumerator JumpStretch()
{
    // squash before jump
    transform.localScale = squashScale;
    yield return new WaitForSeconds(squashTime);

    // stretch at takeoff
    transform.localScale = stretchScale;
    yield return new WaitForSeconds(stretchTime);

    // back to normal
    transform.localScale = originalScale;
}




    public virtual bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
    {
        upwardForce = upwardForce/2;
        knockbackForce = knockbackForce/2;
        

        float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

        //set x velocity to 0 for smoother knockback
        rb.linearVelocityX = 0f;

        //direction is angled a little bit upward for pazzaz
        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;


        //force applied to enemy
        rb.AddForce(force, ForceMode2D.Impulse);

        
        return true;
    }
}

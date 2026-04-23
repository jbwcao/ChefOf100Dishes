using System.Collections;
using UnityEngine;

public class MoonRockMove : MonoBehaviour, IKnockbackable
{

    private Rigidbody2D rb;
    public float knockbackRecoverySpeed = 12f;
    private Coroutine knockbackRecoveryRoutine;

    public float bounceMultiplier = 1f; // 1 = same speed, 0.9 = slight loss
    public LayerMask bounceLayers;
    private Vector2 lastVel; 
    private float rotationSpeed = 10f;

    public bool explodeOut = false;
    public float startingForce = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        if (explodeOut)
        {
            Vector2 force = new Vector2(Random.Range(-1f,0.1f),Random.Range(-1f,1f)) * startingForce;
            //launch in a random direction
            rb.AddForce(force, ForceMode2D.Impulse);
            knockbackRecoveryRoutine = StartCoroutine(RecoverFromKnockback());
        }
    }

    void FixedUpdate()
    {
        lastVel = rb.linearVelocity;
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

    }

    public virtual bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
    {
         float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;
         upwardForce = Random.Range(-0.25f,0.25f);

        //set x velocity to 0 for smoother knockback
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;
        rb.AddForce(force, ForceMode2D.Impulse);

        if (knockbackRecoveryRoutine != null)
        {
            StopCoroutine(knockbackRecoveryRoutine);
        }

        knockbackRecoveryRoutine = StartCoroutine(RecoverFromKnockback());

        return true;
    }


    private IEnumerator RecoverFromKnockback()
    {
        float hitRotation = 10f;
        while (rb.linearVelocity.magnitude > 0.05f)
        {
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                knockbackRecoverySpeed * Time.deltaTime
            );
            transform.Rotate(0, 0, hitRotation * Time.deltaTime);

            yield return null;
        }

        /*while(hitRotation > 1)
        {
            transform.Rotate(0, 0, hitRotation * Time.deltaTime);
            hitRotation = Mathf.Lerp(hitRotation, 0, );//should lerp over Time/delta.time
        }*/

        rb.linearVelocity = Vector2.zero;
        knockbackRecoveryRoutine = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & bounceLayers) == 0)
        {
            return;
        }

        Vector2 velocity = lastVel;

        if (velocity.sqrMagnitude < 0.001f)
        {
            return;
        }

        Vector2 normal = collision.contacts[0].normal;

        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
        {
            velocity.x *= -1f;
        }
        else
        {
            velocity.y *= -1f;
        }

        rb.linearVelocity = velocity * bounceMultiplier;
        knockbackRecoveryRoutine = StartCoroutine(RecoverFromKnockback());
    }
}

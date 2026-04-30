using System.Collections;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

public class CornFire : TomatoLob, IKnockbackable
{
    //if player is in range, stop moving, fire high velocity shots coninueously at the player
    //shots should break if they hit a wall/platform
    //once player leaves range, begin idle flying again
    public float timeBeforeMoving = 0.5f;
    public FlyingIdle movementScript;


    public float firstAttackDelay = 3.75f;

    private bool playerAlerted = false;
    private bool waitingToAttack = false;
    private Transform currentTarget;


    public float knockbackRecoverySpeed = 12f;
    private Coroutine SartupMovement;
    private Coroutine AttackDelay;
    private Coroutine knockbackRecoveryRoutine;
    private Animator animatorr;

    void Start()
    {
        animatorr = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void lookAround()
    {
        Transform foundTarget = FindVisiblePlayer();

        if (foundTarget == null)
        {
            playerAlerted = false;
            currentTarget = null;
            waitingToAttack = false;

            if(movementScript.enabled != true)
            {
                SartupMovement = StartCoroutine(StartupMovement());
            }
            

            return;
        }

        currentTarget = foundTarget;

        if (transform.position.x < currentTarget.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        if (attackCooldown > 0)
        {
            return;
        }

        if (!playerAlerted)
        {
            playerAlerted = true;

            if (!waitingToAttack)
            {
                AttackDelay = StartCoroutine(DelayedAttack());
            }
        }
        else
        {
            if (!attacking && !waitingToAttack)
            {
                StartCoroutine(Attack(currentTarget.position));
            }
        }
    }


    private IEnumerator DelayedAttack()
        {
            waitingToAttack = true;

            if(SartupMovement != null)
            {
                //stop overlap
                StopCoroutine(SartupMovement);
                SartupMovement = null;
                
            }


            if (movementScript != null && movementScript.enabled)
            {
                movementScript.enabled = false;
            }



            animatorr.Play("CornAim", 0, 0f);
            yield return new WaitForSeconds(firstAttackDelay);

            waitingToAttack = false;

            if (currentTarget != null && attackCooldown <= 0 && !attacking)
            {
                animatorr.Play("CornFire", 0, 0f);
                StartCoroutine(Attack(currentTarget.position));
            }
        }

    private IEnumerator StartupMovement()
    {
        
        yield return new WaitForSeconds(timeBeforeMoving);
        if(movementScript != null && SartupMovement != null)
        {   
            rb.linearVelocity = Vector2.zero;
            animatorr.Play("CornUnaim", 0, 0f);
            movementScript.enabled = true;
        }
        
    }

    private Transform FindVisiblePlayer()
    {
        Collider2D[] viewArea = Physics2D.OverlapCircleAll(transform.position, detectionRadius, searchMask);

        foreach (Collider2D target in viewArea)
        {
            if (!target.CompareTag("Player"))
            {
                continue;
            }

            RaycastHit2D blockHit = Physics2D.Linecast(transform.position, target.transform.position, blockMask);

            if (blockHit.collider != null)
            {
                continue;
            }

            return target.transform;
        }

        return null;
    }


    public virtual bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
    {
         float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

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
        while (rb.linearVelocity.magnitude > 0.05f)
        {
            rb.linearVelocity = Vector2.MoveTowards(
                rb.linearVelocity,
                Vector2.zero,
                knockbackRecoverySpeed * Time.deltaTime
            );

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        knockbackRecoveryRoutine = null;
    }
}

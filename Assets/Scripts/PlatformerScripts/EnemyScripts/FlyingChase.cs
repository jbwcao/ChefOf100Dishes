using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class FlyingChase : MonoBehaviour, IKnockbackable
{
    public float agroRadius = 5f;
    public float deagroRadius = 7f;
    public float agroSpeed = 2f;
    public float searchIntervals = 0.25f;

    public float knockbackTime = 0.15f;
    private bool stopMoving = false;



    private bool isChasing = false;
    public FlyingIdle idleScript; 
    public LayerMask playerLayer;
    private Transform player;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(SearchForPlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isChasing && player != null && !stopMoving)
        {
            float distance = Vector2.Distance(rb.position, player.position);
        if (distance <= 0.01) return;
            Vector2 nextPosition = Vector2.MoveTowards(
            rb.position,
            player.position,
            agroSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(nextPosition);
        }
    }

    IEnumerator SearchForPlayer()
    {
        while(true){
            //check to see if player is in range and to agro
            while (!isChasing)
            {
                yield return new WaitForSeconds(searchIntervals);


                Collider2D[] viewArea = Physics2D.OverlapCircleAll(transform.position, agroRadius, playerLayer);
                foreach (Collider2D hit in viewArea) {
                    if (hit.CompareTag("Player"))
                    {
                        Debug.Log("Player Found!");
                        //TODO add a alert animation + sfx
                        player = hit.transform;
                        isChasing = true;
                        //disable Flying idle
                        idleScript.enabled = false;
                    }
                }
            }


            //check if needed to de-argro
            while (isChasing)
            {
                Collider2D[] viewArea = Physics2D.OverlapCircleAll(transform.position, deagroRadius, playerLayer);
                //bool playerGone = true;
                //player is not in view 
                if (viewArea.Length == 0)
                {
                    player = null;
                    isChasing = false;

                    //if knockback is happening
                    CancelKnockback();


                    //enable Flying idle
                    idleScript.SetIdleCenter(rb.position);
                    idleScript.enabled = true;
                }

            
                yield return new WaitForSeconds(searchIntervals);
            }

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, agroRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, deagroRadius);

    }




    public virtual bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
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
        return true;
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
    }
}

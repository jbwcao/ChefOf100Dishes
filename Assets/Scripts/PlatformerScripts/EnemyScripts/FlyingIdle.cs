using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class FlyingIdle : MonoBehaviour//, IKnockbackable
{   
    //public float moveSpeed = 1f;
    public float movementSmoothing = 0.5f;
    public float restIntervals = 0.5f;
    
    
    public float idleCircleRadius = 2f;
    private Vector2 startingPoint;
   

    private Coroutine flyRoutine;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    // choose a nearby point lazily fly over to it
    // wait a little bit, then choose a new point in the idle circle 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        startingPoint = transform.position;
        //on start log an area around begining position
        //flyRoutine = StartCoroutine(franticFly());
        
    }

    void OnEnable()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        startingPoint = transform.position;

        if (flyRoutine == null)
        {
            flyRoutine = StartCoroutine(franticFly());
        }
    }

    void OnDisable()
    {
        if (flyRoutine != null)
        {
            StopCoroutine(flyRoutine);
            flyRoutine = null;
        }
    }

     public void SetIdleCenter(Vector2 newCenter)
    {
        startingPoint = newCenter;
    }




    // Update is called once per frame
    void Update()
    {
        
    }

    
    private Vector2 pickPoint()
    {
        return startingPoint + Random.insideUnitCircle * idleCircleRadius;
    }



    //moves kinda nimbly
    //bug, after de-aggroing 

        private  IEnumerator franticFly(){
        while(true){
            Vector2 velocity = Vector2.zero;
            //pick a point from a cirlce
            Vector2 targetPoint = pickPoint();
            
            sr.flipX = targetPoint.x < rb.position.x? false : true;
            
            
            //Vector2.Lerp towards target point
            while (Vector2.Distance(rb.position, targetPoint) > 0.05f)
            {
                //smoothly move towards 
                Vector2 newPosition = Vector2.SmoothDamp(
                rb.position,
                targetPoint,
                ref velocity,
                movementSmoothing);

                rb.MovePosition(newPosition);
                yield return null;
                
            }
            //once reached lazily decelerate
            
            rb.MovePosition(targetPoint);
            //rest for an interval
            yield return new WaitForSeconds(restIntervals);
        }
    }
    
     private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startingPoint, idleCircleRadius);
    }

}

using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class FlyingIdle : MonoBehaviour
{   
    public float moveSpeed = 1f;
    public float movementSmoothing = 0.5f;
    public float restIntervals = 0.5f;
    
    
    public float idleCircleRadius = 2f;
    private Vector2 startingPoint;
    private Vector2 targetPoint;

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
        StartCoroutine(Idle());
    }

    void OnEnable()
    {
        startingPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private Vector2 pickPoint()
    {
        return startingPoint + Random.insideUnitCircle * idleCircleRadius;
    }

    private  IEnumerator Idle()
    {
        //restart after reaching a point
        while (true)
        {
            Debug.Log("Picking Point");
            yield return StartCoroutine(franticFly());

            //rest for an interval
            yield return new WaitForSeconds(restIntervals);
            
        }
    }


    //moves kinda nimbly
    private  IEnumerator franticFly(){
        Vector2 velocity = Vector2.zero;
        //pick a point from a cirlce
        targetPoint = pickPoint();
        
        sr.flipX = targetPoint.x < transform.position.x? false : true;
        
        
        //Vector2.Lerp towards target point
        while (Vector2.Distance(transform.position, targetPoint) > 0.05f)
        {
            //smoothly move towards 
            Vector2 newPosition = Vector2.SmoothDamp(
            transform.position,
            targetPoint,
            ref velocity,
            movementSmoothing);

            rb.MovePosition(newPosition);
            yield return null;
            
        }
        //once reached lazily decelerate
        
       rb.MovePosition(targetPoint);
    }

}

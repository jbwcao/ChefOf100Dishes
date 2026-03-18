using UnityEngine;
using UnityEngine.InputSystem;
public class playerAttack : MonoBehaviour
{
    public int damage = 1;
    public float attackSpeed = 0.75f;
    public Vector2 attackSize =  new Vector2(1f,0.5f);

    public float range = 1.15f;
    
    public InputAction playerAttacks;//TODO figure this new input system out for getting an attack input
    public Transform attackPoint;
    public LayerMask enemyLayers;
    
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Attacked");
            attack();
        }
   //     if(UnityEngine.Input("attack")){
    //        attack();
    //    } 
    }

    private void OnEnable()
    {
        playerAttacks.Enable();
    }
    private void OnDisable()
    {
        playerAttacks.Disable();
    }

    void attack()
    {
        //animator.SetTrigger("attack") // play attack animation
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, enemyLayers);
        foreach(Collider2D enemy in enemiesHit){
            //enemy.gameObject.GetComponent<>;
            Debug.Log("Enemy hit: " + enemy.name);

        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return ;
        }
        Gizmos.DrawWireCube(attackPoint.position, attackSize);
    }
}

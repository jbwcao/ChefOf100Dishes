using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public int damage = 1;
    public float attackSpeed = 0.75f;
    public Vector2 attackSize = new Vector2(1f, 0.5f);
    public float range = 1.15f;

    [Header("Enemies")]
    public LayerMask enemyLayers;

    InputAction attackAction;
    InputAction moveAction;
    Rigidbody2D rb;

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (attackAction.WasPerformedThisFrame())
        {
            Attack(PlayerMovement.FacingDirection * range);
        }
    }

    void Attack(float attackDirectionOffset)
    {
        //animator.SetTrigger("attack") // TODO: play attack animation

        Vector2 attackCenter = (Vector2) transform.position + new Vector2(attackDirectionOffset, 0);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers);

        foreach(Collider2D enemy in enemiesHit)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            Destroy(enemy.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + new Vector2(range, 0), attackSize);
        Gizmos.DrawWireCube((Vector2) transform.position - new Vector2(range, 0), attackSize);
    }
}

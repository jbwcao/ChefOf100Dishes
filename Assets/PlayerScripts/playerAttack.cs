using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public int damage = 2;
    public float attackSpeed = 0.75f;
    private float attackCooldown = 0f;
    public Vector2 attackSize = new Vector2(1f, 0.5f);
    public float range = 1.15f;

    [Header("Enemies")]
    public LayerMask enemyLayers;

    InputAction attackAction;
    InputAction moveAction;
    Rigidbody2D rb;

    private int directionFacing; // 1 for facing right, -1 for facing left

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();

        directionFacing = 1;
    }

    void Update()
    {
        float xInput = moveAction.ReadValue<Vector2>().x;
        directionFacing = xInput > 0 ? 1 : (xInput < 0 ? -1 : directionFacing);


        //timer for reseting attack
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }


        if (attackAction.WasPerformedThisFrame() && attackCooldown <= 0f)
        {
            Debug.Log("Attacked, " + $"Direction Facing: {directionFacing}");

            float attackDirection = rb.linearVelocityX >= 0 ? 1 : -1;
            Attack(attackDirection * range);
        }
    }

    void Attack(float attackDirectionOffset)
    {
        //animator.SetTrigger("attack") // TODO: play attack animation

        Vector2 attackCenter = (Vector2) transform.position + new Vector2(attackDirectionOffset, 0);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers);
        attackCooldown = attackSpeed;

        foreach(Collider2D enemy in enemiesHit)
        {
            Debug.Log("Enemy hit: " + enemy.name);
            enemy.gameObject.GetComponent<EnemyHitbox>().takeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + new Vector2(range, 0), attackSize);
        Gizmos.DrawWireCube((Vector2) transform.position - new Vector2(range, 0), attackSize);
    }
}

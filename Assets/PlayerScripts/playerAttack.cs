using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    public int damage = 2;
    public float attackSpeed = 0.75f;
    private float attackCooldown = 0f;
    public float knockBackUpwardsPower = 2f;
    public float knockBackPower = 8f;
    public Vector2 attackSize = new Vector2(1f, 0.5f);
    public float range = 1.15f;

    [Header("Enemies")]
    public LayerMask enemyLayers;

    InputAction attackAction;
    InputAction moveAction;
    Rigidbody2D rb;
    Animator animator;

    
    public Animator slashAnimator;
    public SpriteRenderer slashSr;

    private int directionFacing; // 1 for facing right, -1 for facing left

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        directionFacing = 1;

        slashSr.sprite = null;
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
            Attack(directionFacing * range);
        }
    }

    void Attack(float attackDirectionOffset)
    {
        //attack animation looks janky with moving
        attackCooldown = attackSpeed;
        Vector2 attackCenter = (Vector2) transform.position + new Vector2(attackDirectionOffset, 0);
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackCenter, attackSize, 0f, enemyLayers);
        animator.Play("Attack", 0, 0f);
        slashAnimator.Play("SwingFlare", 0, 0f);

        foreach(Collider2D enemy in enemiesHit)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit: " + enemy.name);
                enemy.gameObject.GetComponent<EnemyMovement>().applyKnockback(transform.position, knockBackUpwardsPower, knockBackPower);
                enemy.gameObject.GetComponent<EnemyHitbox>().takeDamage(damage);
            }
           
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + new Vector2(range, 0), attackSize);
    }
}

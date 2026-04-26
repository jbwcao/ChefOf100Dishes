using UnityEngine;

public class PotatoEnemy : EnemyMovement
{
    [Header("Potato Settings")]
    public Transform player;
    public float detectionRange = 8f;
    public float arrivalThreshold = 0.5f;
    public EnemyHitbox hitbox;
    private int normalDamage;

    Animator anim;
    bool isChasing = false;

    protected override void Start()
    {
        base.Start();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        anim = GetComponent<Animator>();
        anim.Play("PotatoBounce");
        normalDamage = hitbox.contactDamage;
        
    }
    public override bool applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 8f)
{
    if (isChasing) return false; // underground, can't be hit
    return base.applyKnockback(hitFromPosition, upwardForce, knockbackForce);
}

    protected override void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float xDist = Mathf.Abs(player.position.x - transform.position.x);

        if (distToPlayer > detectionRange)
        {
            if (isChasing)
            {
                isChasing = false;
                anim.Play("PotatoBounce");
                hitbox.contactDamage = normalDamage;
            }
        }
        else if (xDist <= arrivalThreshold)
        {
            if (isChasing)
            {
                isChasing = false;
                anim.Play("PotatoBounce");
                hitbox.contactDamage = normalDamage;
            }
        }
        else
        {
            if (!isChasing)
            {
                isChasing = true;
                anim.Play("Chase");
                hitbox.contactDamage = 0;

            }
            currentDir = player.position.x > transform.position.x ? 1 : -1;
            basic_move();
        }
    }
}
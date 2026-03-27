using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float airAcceleration;
    public float groundAcceleration;
    public float maxHorizontalSpeed;

    [Header("Jumping")]
    public float jumpStrength;
    public float maxJumpHeight;
    public float jumpBuffer;
    public float coyoteTime;

    [Header("Physics")]
    public float gravity;

    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody2D rb;
    Collider2D col;

    private bool perventControl = false;
    public float knockbackTime = 0.15f;
    private float coyoteTimer;
    private float jumpBufferTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        coyoteTimer = 0;
        jumpBufferTimer = 0;
        rb.gravityScale = gravity;
    }

    void Update()
    {
        // could movement be moved into it's own method thats called in update?
        if (!perventControl)
        {
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, col.bounds.min.y - 0.01f), new Vector2(0, -1), 0.1f);

            bool onGround = hit.collider == null? false : hit.collider.CompareTag("Platform");
            float xInput = moveValue.x;

            if (xInput == 0)
            {
                // TODO: Implement a tweakable deceleration value
                rb.linearVelocityX = 0;
            }
            else {
                rb.linearVelocityX += (onGround ? groundAcceleration : airAcceleration) * xInput * Time.deltaTime;

                rb.linearVelocityX = rb.linearVelocityX > maxHorizontalSpeed ? maxHorizontalSpeed : rb.linearVelocityX;
                rb.linearVelocityX = rb.linearVelocityX < -maxHorizontalSpeed ? -maxHorizontalSpeed : rb.linearVelocityX;
            }

            jumpBufferTimer = jumpAction.WasPressedThisFrame() ? jumpBuffer : Math.Max(jumpBufferTimer - Time.deltaTime, 0);
            coyoteTimer = onGround ? coyoteTime : Math.Max(coyoteTimer - Time.deltaTime, 0);

            if (coyoteTimer > 0 && jumpBufferTimer > 0)
            {
                coyoteTimer = 0;
                jumpBufferTimer = 0;
                StartCoroutine(Jump());
            }
        }
        
    }

    //BUG: itemdrops from enemies pervent jumping when nearby
    private IEnumerator Jump()
    {
        rb.linearVelocityY = jumpStrength;

        float startingY = transform.position.y;
        while(transform.position.y - startingY < maxJumpHeight && jumpAction.IsPressed())
        {
            rb.gravityScale = 0;
            yield return null;
        }
        
        rb.gravityScale = gravity;
    }


     public void applyKnockback(Vector2 hitFromPosition, float upwardForce = 2f, float knockbackForce = 7f)
    {
        perventControl = true;

        float xDir = transform.position.x > hitFromPosition.x ? 1f : -1f;

        //set x velocity to 0 for smoother knockback
        rb.linearVelocityX = 0f;

        //direction is angled a little bit upward for pazzaz
        Vector2 force = new Vector2(xDir, upwardForce).normalized * knockbackForce;


        //force applied to enemy
        rb.AddForce(force, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), knockbackTime);
    }

    void EndKnockback()
    {
        perventControl = false;
    }
}

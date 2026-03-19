using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

[DisallowMultipleComponent]
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
    public float peakJumpGravity;

    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody2D rb;
    Collider2D col;

    public static int FacingDirection { get; private set; }

    private float coyoteTimer;
    private float jumpBufferTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");

        FacingDirection = -1;
        coyoteTimer = 0;
        jumpBufferTimer = 0;
        rb.gravityScale = gravity;
    }

    void Update()
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
            FacingDirection = (int) Mathf.Ceil(xInput);
            transform.localScale = new Vector2(-6 * FacingDirection, transform.localScale.y);
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

    private IEnumerator Jump()
    {
        rb.linearVelocityY = jumpStrength;

        float startingY = transform.position.y;
        float elapsedJump = 0;

        while (transform.position.y - startingY < maxJumpHeight && jumpAction.IsPressed())
        {
            elapsedJump = (transform.position.y - startingY) / maxJumpHeight;
            rb.gravityScale = Mathf.Lerp(0, peakJumpGravity, elapsedJump);

            Debug.Log(elapsedJump);

            if (rb.linearVelocityY == 0)
            {
                break;
            }
            yield return null;
        }
        
        rb.gravityScale = gravity;
    }
}

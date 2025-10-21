
using System;
using JetBrains.Annotations;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour

{
    [SerializeField] private Rigidbody2D rb;   // <- must be Rigidbody2D (capital D)
    public Animator animator;
    [SerializeField] private float moveSpeed = 5f;

    bool isFacingRight = true;
    public ParticleSystem smokeFX;

    [Header("Movement")]
    private float horizontalMovement;

    [Header("GroundCheck")]
    public Transform groundCheckpos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("WallCheck")]
    public Transform wallCheckpos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //WallJump
    bool isWalljumping;
    float wallJumpDirection;
    float wallJumpTime = 0.05f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 5f);


    [Header("Jumping")]
    [SerializeField] public float jumpPower = 10f;
    [SerializeField] public int maxJumps = 2;
    int jumpsRemaining;
    void Update()
    {

        
        GroundCheck();
        ProcessWallSlide();
        ProcessWallJump();
        if (!isWalljumping)
        {

            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            flip();
            smokeFX.Play();
        }


        // Set animation speed based on movement
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));
        smokeFX.Play();
    }
    

    // Called automatically by the new Input System if bound properly
    public void Move(InputAction.CallbackContext context)
    {
        // Correct type name: CallbackContext, not Callbackcontext
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
{
        if (context.performed && jumpsRemaining > 0)
        {
            //full jump
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpsRemaining--;
        }
        else if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            jumpsRemaining--;
        }
    //walljump
    if(context.performed && wallJumpTimer >0f)
       {
            isWalljumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;

            Invoke(nameof(CancelwallJump), wallJumpTime + 0.1f);  // walljump 0.5f = jump again at 0.6f
            if (transform.localScale.x != wallJumpDirection)
            {
             isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            }

        }
}


    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckpos.position, groundCheckSize, 0f, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }

    }
    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckpos.position, wallCheckSize, 0f, wallLayer);
    }

    private void ProcessWallSlide()

    {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)

        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Math.Max(rb.linearVelocityY, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

    }

    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWalljumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelwallJump));

        }
        else if (wallJumpTimer > 0)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }
    
    private void CancelwallJump()
    {
        isWalljumping = false;
    }
    private void flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
            smokeFX.Play();
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckpos.position, groundCheckSize);
   
        Gizmos.color = Color.navyBlue;
        Gizmos.DrawWireCube(wallCheckpos.position, wallCheckSize);
    }
}


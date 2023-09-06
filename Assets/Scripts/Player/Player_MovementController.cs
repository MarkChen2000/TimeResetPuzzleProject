using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_MovementController : MonoBehaviour
{
    // References
    [SerializeField] Transform playerGraphx, groundCheckSpot;
    InputMaster inputControl;
    [SerializeField] Rigidbody2D m_Rigid;

    [Space]
    // Values
    [SerializeField] bool isFacingRight = true; // by default sprite facing.
    [SerializeField] float maxMoveSpeed = 10f, accelerationRate = 0.5f, deccelerationRate = 0.5f, moveForce = 10f;
    [SerializeField] float groundFrictionAmount = 0.5f, airDragAmount = 0.3f;

    [Space]
    [SerializeField] LayerMask GroundLayers;
    [SerializeField] float jumpForce = 10f, jumpCutMultiplier = 0.5f;
    [SerializeField] float falltimeGravityMultiplier = 1.5f;

    // Start is called before the first frame update
    void Awake() 
    {
        InputSystemInitialize();

        originGravityScale = m_Rigid.gravityScale;
    }

    void InputSystemInitialize()
    {
        inputControl = new InputMaster();
        inputControl.Enable();

        //inputControl.Player.Move.performed += content => ExcuteMovement(content.ReadValue<Vector2>());
        inputControl.Player.Jump.performed += content => ExcuteJump();
    }

    //bool isGrounded = false;
    Vector2 movedirection;

    void Update() 
    {
        //isGrounded = GroundCheck();
        CheckInputs();
        CheckandSetGravity();
        JumpCheck();
    }

    void FixedUpdate() 
    {
        ExcuteMovement();
        ImplementDrag();
    }

    void CheckInputs()
    {
        //movedirection = new Vector2( Input.GetAxisRaw("Horizontal"), 0f);

        movedirection = inputControl.Player.Move.ReadValue<Vector2>();

        if (inputControl.Player.Jump.WasReleasedThisFrame()) JumpCut();
    }

    float originGravityScale;
    void CheckandSetGravity()
    {
        if ( m_Rigid.velocity.y < 0f ) // when character falling.
            m_Rigid.gravityScale = falltimeGravityMultiplier * originGravityScale;
        else
            m_Rigid.gravityScale = originGravityScale;
    }

    void ExcuteMovement() 
    {
        CheckRotate();

        // simple movement method.
        //m_Rigid.velocity = new Vector2(movedirection.x * moveSpeed, m_Rigid.velocity.y); 

        // improved movement method.
        float targetSpeed = movedirection.x * maxMoveSpeed;
        float speedDif = targetSpeed - m_Rigid.velocity.x; // speed difference
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelerationRate : deccelerationRate;
        // This means that if absoluted targetspeed > 0.01 then accelerationRate assign to accelerationrate, vice versa.
        float movementForce = Mathf.Pow( Mathf.Abs(speedDif) * accelRate, moveForce) * Mathf.Sign(speedDif);
        // "sign" means > 0 return 1, < 0 return -1.
        m_Rigid.AddForce(movementForce * Vector2.right);
    }

    void ImplementDrag() // fictional friction.
    {
        float dragAmount;

        if (GroundedCheck())
            dragAmount = groundFrictionAmount;
        else
            dragAmount = airDragAmount;

        Vector2 dragForce = dragAmount * m_Rigid.velocity;
        dragForce.x = Mathf.Min( Mathf.Abs(dragForce.x), Mathf.Abs(m_Rigid.velocity.x) );
        dragForce.y = Mathf.Min( Mathf.Abs(dragForce.y), Mathf.Abs(m_Rigid.velocity.y) );
        // only implement drag when character is moving.

        dragForce.x *= Mathf.Sign( m_Rigid.velocity.x );
        dragForce.y *= Mathf.Sign( m_Rigid.velocity.y );

        m_Rigid.AddForce( -dragForce, ForceMode2D.Impulse );
    }

    void CheckRotate() 
    {
        if ( movedirection.x > 0f && !isFacingRight ) 
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f,1f,1f);
            isFacingRight = !isFacingRight;
        }
        if ( movedirection.x < 0f && isFacingRight ) 
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
            isFacingRight = !isFacingRight;
        }
    }

    Vector2 groundCheckBoxSize = new Vector2(0.3f,0.1f);
    bool GroundedCheck() 
    {
        if (Physics2D.OverlapBox(groundCheckSpot.position, groundCheckBoxSize, 0f, GroundLayers))
        {
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireCube(groundCheckSpot.position, groundCheckBoxSize);
    }

    bool isJumping = false;
    void ExcuteJump() 
    {
        if ( !GroundedCheck() ) return;

        m_Rigid.AddForce( Vector2.up * jumpForce , ForceMode2D.Impulse );
        isJumping = true;
    }

    void JumpCut()
    {
        if ( m_Rigid.velocity.y > 0f && isJumping )
            m_Rigid.AddForce(Vector2.down * m_Rigid.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
    }

    void JumpCheck()
    {
        if (isJumping && m_Rigid.velocity.y < 0f) // if player is falling, then it means player is not jumping.
            isJumping = false;
    }

}

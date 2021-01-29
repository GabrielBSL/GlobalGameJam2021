using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Serializable variables
    [Header("Movement")]
    [SerializeField] float horizontalSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] int jumpLimit = 2;
    
    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashTime = 0.3f;
    [SerializeField] int dashLimit = 1;

    //Technical variables
    [Header("Tech Gameplay Variables")]
    [SerializeField] float jumpTime = 0.5f;
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float jumpExtensionTime = 0.3f;
    [SerializeField] float maxFallVelocity = 10f;

    //Private variables
    private float horizontalMovement;
    private float jumpExtensionTimer = 0;
    private float jumpTimeCounter = 0;
    private float dashTimerCounter = 0;
    private int jumpLimitCounter = 1;
    private int dashLimitCounter = 0;
    private bool isJumping = false;
    private bool isDashing = false;
    private bool isGrounded = false;
    private bool isFacingRight = true;
    private bool isJumpExtensioning = false;
    private Vector2 dashDirectionValues;

    //Player Components
    private Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Dash();
        SetFacingDirection();

        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            if(dashLimitCounter < dashLimit)
            {
                isDashing = true;
            }
        }
        horizontalMovement = Input.GetAxis("Horizontal");
        dashDirectionValues = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Jump();
    }

    private void FixedUpdate()
    {
        if (isDashing) { 
            rigidbody.gravityScale = 0; 
            return;
        }
        else
        {
            rigidbody.gravityScale = gravityScale;
        }

        Move();
        fall();
        jumpExtension();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpLimitCounter < jumpLimit)
        {
            jumpLimitCounter++;
            jumpTimeCounter = 0;
            isJumping = true;
            rigidbody.velocity = Vector2.up * jumpForce;
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            if (jumpTimeCounter <= jumpTime && isJumping)
            {
                jumpTimeCounter += Time.deltaTime;
                rigidbody.velocity = Vector2.up * jumpForce;
            }
            else
            {
                isJumping = false;
            }
        }

        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }

    private void SetFacingDirection()
    {
        if (horizontalMovement < 0)
        {
            isFacingRight = false;
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if (horizontalMovement > 0)
        {
            isFacingRight = true;
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void Move()
    {
        rigidbody.velocity = new Vector2(horizontalMovement * horizontalSpeed, rigidbody.velocity.y);
    }

    private void Dash()
    {
        if(isDashing)
        {
            if (dashTimerCounter < dashTime)
            {
                dashTimerCounter += Time.deltaTime;
                Vector2 dashDirection = dashDirectionCorrection(dashDirectionValues);
                rigidbody.velocity = dashDirection * dashSpeed;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
                isDashing = false;
                
                dashTimerCounter = 0;

                if (isGrounded)
                {
                    dashLimitCounter=0;
                }
                else
                {
                    dashLimitCounter++;
                }
            }
        }
    }

    private Vector2 dashDirectionCorrection(Vector2 dashDirectionValues)
    {
        float x = 0;
        float y = 0;

        if(dashDirectionValues == new Vector2(0, 0))
        {
            if(isFacingRight) return new Vector2(1, 0);
            else return new Vector2(-1, 0);
        }

        if (dashDirectionValues.x > 0) x = 1;
        else if (dashDirectionValues.x < 0) x = -1;
        else x = 0;

        if (dashDirectionValues.y > 0) y = 1;
        else if (dashDirectionValues.y < 0) y = -1;
        else y = 0;

        if(x != 0 && y != 0)
        {
            float diagonalForce = 1/Mathf.Sqrt(2);

            if (x < 0) x = -diagonalForce;
            else x = diagonalForce;

            if (y < 0) y = -diagonalForce;
            else y = diagonalForce;
        }
        return new Vector2(x, y);
    }

    private void fall()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Max(rigidbody.velocity.y, maxFallVelocity * -1f));
    }

    private void jumpExtension()
    {
        if(isJumpExtensioning)
        {
            jumpExtensionTimer += Time.deltaTime;

            if(jumpExtensionTimer >= jumpExtensionTime)
            {
                isJumpExtensioning = false;

                if(jumpLimitCounter == 0)
                    jumpLimitCounter++;
            }
        }
        else
        {
            jumpExtensionTimer = 0;
        }
    }

    public void SetIsGrounded(bool groundCheck)
    {
        isGrounded = groundCheck;
        
        if (isGrounded)
        {
            dashLimitCounter = 0;
            isJumpExtensioning = false;
            jumpLimitCounter = 0;
        }
        else if(jumpLimitCounter == 0)
        {
            isJumpExtensioning = true;
        }
    }
}

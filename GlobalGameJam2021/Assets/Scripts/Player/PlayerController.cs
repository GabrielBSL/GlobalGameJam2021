using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    class AudioSample
    {
        public string name;
        public AudioClip[] audios;
    }

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

    [Header("SoundEffects")]
    [SerializeField] AudioSample[] SoundEffects;

    //Private variables
    private float horizontalMovement;
    private float jumpExtensionTimer = 0;
    private float dashTimerCounter = 0;
    private float jumpTimeCounter = 0;
    private int jumpLimitCounter = 1;
    private int dashLimitCounter = 0;
    private bool isDead = false;
    private bool isJumping = false;
    private bool isFalling = false;
    private bool isDashing = false;
    [SerializeField] private bool isGrounded = false;
    private bool dashPressed = false;
    private bool holdingJump = false;
    private bool isFacingRight = true;
    private bool startJumping = false;
    private bool releasedJump = false;
    private bool isJumpExtensioning = false;

    private Vector2 dashDirectionValues;

    //Player Components
    private Rigidbody2D rigidbody;
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        SetFacingDirection();
        
        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            if (dashLimitCounter < dashLimit)
            {
                dashPressed = true;
            }
        }
        horizontalMovement = Input.GetAxis("Horizontal");
        dashDirectionValues = GetKeyDirection();
        CheckJumpInputs();
    }

    private void FixedUpdate()
    {
        Dash();

        if (isDashing) { 
            rigidbody.gravityScale = 0;
            return;
        }
        else
        {
            rigidbody.gravityScale = gravityScale;
        }

        Jump();
        Move();
        Fall();
        JumpExtension();
        CheckIdle();
    }

    private void CheckJumpInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startJumping = true;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            holdingJump = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            releasedJump = true;
        }
    }

    private static Vector2 GetKeyDirection()
    {
        int x = 0;
        int y = 0;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) x = -1;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) x = 1;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) y = 1;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) y = -1;

        return new Vector2(x, y);
    }

    private void CheckIdle()
    {
        if(rigidbody.velocity == Vector2.zero)
        {
            SetAnimation("");
        }
    }

    private void Jump()
    {
        if (startJumping && jumpLimitCounter < jumpLimit)
        {
            startJumping = false;
            jumpLimitCounter++;
            jumpTimeCounter = 0;
            isJumping = true;
            rigidbody.velocity = Vector2.up * jumpForce;

            if (jumpLimitCounter == 1)
            {
                SetAnimation("jump");
                PlaySFX("jump");
            }
            else
            {
                SetAnimation("doublejump");
                PlaySFX("doublejump");
            }
        }

        else if (holdingJump)
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

        else if (releasedJump)
        {
            releasedJump = false;
            holdingJump = false;
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
        if(rigidbody.velocity.x != 0 && rigidbody.velocity.y == 0)
        {
            SetAnimation("walk");
        }
    }

    private void Dash()
    {
        if (dashPressed)
        {
            isDashing = true;
            dashPressed = false;
        }
        if(isDashing)
        {
            if (dashTimerCounter < dashTime)
            {
                if (dashTimerCounter == 0)
                    PlaySFX("dash");

                dashTimerCounter += Time.deltaTime;
                Vector2 dashDirection = dashDirectionCorrection(dashDirectionValues);
                rigidbody.velocity = dashDirection * dashSpeed;
                SetAnimation("dash");
                jumpTimeCounter = jumpTime;
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

    private void Fall()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Max(rigidbody.velocity.y, maxFallVelocity * -1f));
        if(rigidbody.velocity.y < -0.5)
        {
            isFalling = true;
            SetAnimation("fall");
        }
        else
        {
            isFalling = false;
        }
    }

    private void JumpExtension()
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

    private void SetAnimation(string animName)
    {
        animator.SetBool("walk", "walk".Equals(animName));
        animator.SetBool("jump", "jump".Equals(animName));
        animator.SetBool("doublejump", "doublejump".Equals(animName));
        animator.SetBool("fall", "fall".Equals(animName));
        animator.SetBool("death", "death".Equals(animName));
        animator.SetBool("dash", "dash".Equals(animName));
    }

    private void PlaySFX(string name)
    {
        audioSource.Stop();

        for (int i = 0; i < SoundEffects.Length; i++)
        {
            if(SoundEffects[i].name == name)
            {
                int randomSFX = UnityEngine.Random.Range(0, SoundEffects[i].audios.Length);

                audioSource.clip = SoundEffects[i].audios[randomSFX];
                audioSource.Play();
                break;
            }
        }
    }

    public void killPlayer()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;
        isDead = true;
        SetAnimation("death");
        PlaySFX("death");
    }

    //Animation function
    public void animationSound(string name)
    {
        PlaySFX(name);
    }
}

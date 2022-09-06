using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float rushSpeed;
    private float currentSpeed;

    public Rigidbody2D theRB;
    public float jumpForce;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private bool canDoubleJump;

    private Animator anim;
    private SpriteRenderer theSR;

    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    public float bounceForce;
    public float checkRadius;

    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();

        currentSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {



            theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

            //Possible Rush Speed Dirty Fix
            if (Input.GetKey(KeyCode.D) && currentSpeed < rushSpeed)
            {
                currentSpeed += 1 * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D) && currentSpeed > moveSpeed)
            {
                currentSpeed -= 1 * Time.deltaTime;
            }

            //Possible input fix?
            float input = Input.GetAxisRaw("Horizontal");

            float translation = Input.GetAxis("Vertical") * currentSpeed;
            float strafe = Input.GetAxis("Horizontal") * currentSpeed;
            translation *= Time.deltaTime;
            strafe *= Time.deltaTime;

            transform.Translate(strafe, 0, translation);

            if (Input.GetKeyDown("escape"))
                Cursor.lockState = CursorLockMode.None;

            if (isGrounded)
            {
                canDoubleJump = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(4);
                }
                else
                {
                    if (canDoubleJump)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        canDoubleJump = false;
                        AudioManager.instance.PlaySFX(4);
                    }
                }
            }

            if (theRB.velocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.velocity.x > 0)
            {
                theSR.flipX = false;
            }

            isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatIsGround);

            if (isTouchingFront == true && isGrounded == false && input != 0)
            {
                wallSliding = true;
            }

            else
            {
                wallSliding = false;
            }

            if (wallSliding)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, Mathf.Clamp(theRB.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            if (Input.GetButtonDown ("Jump") && wallSliding == true)
            {
                wallJumping = true;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
            }

            if (wallJumping == true)
            {
                theRB.velocity = new Vector2(xWallForce * -input, yWallForce);
            }
        }

        else
        {
            knockBackCounter -= Time.deltaTime;
            if (!theSR.flipX)
            {
                theRB.velocity = new Vector2(-knockBackForce, theRB.velocity.y);
            }

            else
            {
                theRB.velocity = new Vector2(knockBackForce, theRB.velocity.y);
            }
        }

        anim.SetFloat("moveSpeed", Mathf.Abs( theRB.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

 
    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        theRB.velocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("hurt");
    }

    public void Bounce()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
        AudioManager.instance.PlaySFX(4);
    }
}

using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class NicoPlayerTest : MonoBehaviour
{
    private float xInput;
    private bool jumpPressedDown;
    private bool jumpPressed;

    bool isFacingRight = true;

    bool isGliding = false;
    bool glidingPressed;


    private float jumpTimer;
    [SerializeField]
    private float jumpInputTime = 0.1f;

    private float groundedTimer;
    private bool isGrounded;

    [SerializeField]
    private float defaultGravityScale;


    [SerializeField]
    private float glidingGravityScale;
    [SerializeField]
    private float glidingTime;
    private float curGlidingTimer;


    [SerializeField]
    private float coyoteTime = 0.1f;

    [SerializeField]
    private Transform groundCheckTransform;
    [SerializeField]
    private Vector2 groundCheckBoxDimensions;
    [SerializeField]
    private LayerMask groundCheckLayerMask;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Animator camAnim;

    // Debug
    [SerializeField]
    private SpriteRenderer gliderSprite;


    void Start()
    {
    }

    void Update()
    {
        groundedTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        GetUserInput();

        //Collider2D collider = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckBoxDimensions, 0, groundCheckLayerMask);

        // if (collider != null)
        // {
        //     groundedTimer = coyoteTime;
        //     isGrounded = true;
        //     curGlidingTimer = glidingTime;
        // }
        // else
        // {
        //     isGrounded = false;
        // }
        // 
        // if (jumpPressedDown)
        // {
        //     jumpTimer = jumpInputTime;
        // }

        // if (jumpTimer > 0)
        // {
        //     if (groundedTimer > 0)
        //     {
        //         Jump();
        //         jumpTimer = 0;
        //         groundedTimer = 0;
        //     }
        // }

        // isGliding = GlidingCheck();


        // if (isGliding)
        // {
        //     rb.gravityScale = glidingGravityScale;
        //     rb.linearVelocity = Vector2.zero;
        //     gliderSprite.gameObject.SetActive(true);
        //     curGlidingTimer -= Time.deltaTime;
        // }
        // else
        // {
        //     rb.gravityScale = defaultGravityScale;
        //     gliderSprite.gameObject.SetActive(false);
        // }

        // if (isFacingRight)
        // {
        //     if (xInput < 0)
        //     {
        //         FlipPlayer();
        //     }
        // }
        // else // isFacingRight = false
        // {
        //     if (xInput > 0)
        //     {
        //         FlipPlayer();
        //     }
        // }
    }

    void FlipPlayer()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        isFacingRight = !isFacingRight;
    }

    bool GlidingCheck()
    {
        if (!glidingPressed)
            return false;
        if (isGrounded)
            return false;
        if (rb.linearVelocityY >= 0.1f)
            return false;
        if (curGlidingTimer <= 0)
            return false;

        return true;
    }

    void GetUserInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        // xInput = move.action.ReadValue<Vector2>().x;

        jumpPressedDown = Input.GetKeyDown(KeyCode.Space);
        jumpPressed = Input.GetKey(KeyCode.Space);
        glidingPressed = Input.GetKey(KeyCode.Space);

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     jumpTimer = jumpInputTime;
        //     
        // }
    }

    void Jump()
    {
        if (rb.linearVelocity.y > 0.1f)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        ApplySidewardsMovement();
    }

    private void ApplySidewardsMovement()
    {
        // transform.Translate(new Vector3(xInput, 0, 0) * speed);

        // rb.AddForce(new Vector2(xInput, 0) * speed, ForceMode2D.Force);
        rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
    }


    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckBoxDimensions);
    }
}




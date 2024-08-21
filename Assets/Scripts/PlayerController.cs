using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    //Check if player touch ground
    public Transform GroundCheck;
    const float GroundCheckRadius = .2f;
    public LayerMask groundLayer;
    private bool isTouchGround;

    public Collider2D standingCollider; // Player stand or crouch

    //Check if player head touch something
    public Transform OverHeadCheck;
    const float OverHeadCheckRadius = .2f;
    
    //Move character
    private float horizontal;
    private float moveSpeed = 6f; // Character speed
    private float jumpPower = 13f;// Character jump
    private float moveSpeedCrouch = 3f; //Speed when crouch

    // ANIMATION
    private Animator playerAnimation;
    private bool crouch;
    private bool isTouchEnemy;

    //TELEPORTER
    Vector2 checkPointPos;
    private GameObject currentTeleporter; //Position player will tele when player die

    GameController gameController;
    HealthManager m_healthBar;

    //Physics when touch enemy
    private float KBForce = 5f; // Lực đẩy lùi
    private float KBCounter;
    private float KBTotalTime = 0.5f; // Thời gian bị đẩy lùi

    private bool KnockFromRight;

    private float invincibilityDuration = 0.5f;
    private float touchEnemyDuration = 0.5f;
    private bool isInvincible = false;


    void Start()
    {
        checkPointPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        m_healthBar = FindObjectOfType<HealthManager>();

    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        isTouchGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, groundLayer); // isTouchGround

        Button();
        Move();

        if(m_healthBar.healthAmount <= 0f)
        {
            Die();
        }


        playerAnimation.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        playerAnimation.SetFloat("yVelocity", rb.velocity.y);

        playerAnimation.SetBool("OnGround", isTouchGround);     
        playerAnimation.SetBool("Crouch", crouch);
        playerAnimation.SetBool("isTouchEnemy", isTouchEnemy);
    }
   
    private void FixedUpdate()
    {
        if(KBCounter <= 0)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }
        else
        {
            if(KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, rb.velocity.y);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, rb.velocity.y);
            }

            KBCounter -= Time.deltaTime;
        }

        bool isObstructedAbove = Physics2D.OverlapCircle(OverHeadCheck.position, OverHeadCheckRadius, groundLayer);

        // Nếu có vật cản phía trên, bắt buộc phải cúi
        if (isObstructedAbove)
        {
            crouch = true;
        }
        else
        {
            if (Input.GetButton("Crouch"))
                crouch = true;
            else
                crouch = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            KBCounter = KBTotalTime;

            if (collision.transform.position.x <= transform.position.x)
                KnockFromRight = false;
            if (collision.transform.position.x > transform.position.x)
                KnockFromRight = true;

            m_healthBar.takeDamage(10);
            StartCoroutine(InvincibilityCoroutine());
            StartCoroutine(TouchEnemyCoroutine());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dead"))
        {
            m_healthBar.takeDamage(100);
        }

        if (collision.gameObject.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            gameController.AddCherry();
        }
        if (collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            gameController.AddGem();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teleporter"))
        {
            currentTeleporter = null;
        }
    }
    #region Move character and key button
    private void Button()
    {
        if (Input.GetButtonDown("Jump") && isTouchGround) // Jump
        {
            rb.velocity = Vector2.up * jumpPower;
            isTouchGround = false;
        }
        if (Input.GetKeyDown(KeyCode.E)) //Teleporter
        {
            if (currentTeleporter != null)
            {
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }
        if (Input.GetButtonDown("Crouch"))// Cúi xuống( Crouch )
        {
            crouch = true;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }
    private void Move()
    {
        if (isTouchGround)
        {
            standingCollider.enabled = !crouch;
        }
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(OverHeadCheck.position, OverHeadCheckRadius, groundLayer))
                crouch = true;
        }
    }
    private void Flip()
    {
        if (horizontal > 0f)
            transform.localScale = new Vector2(1, 1);
        else if (horizontal < 0f)
            transform.localScale = new Vector2(-1, 1);
    }
    #endregion
    #region Checkpoint
    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
    IEnumerator Respawn(float duration) 
    {
        rb.velocity = new Vector2(0, 0);
        rb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = checkPointPos;
        transform.localScale = new Vector3(1, 1, 1);
        rb.simulated = true;
    }
    #endregion
    # region Make character invincibiltiy whem touch enemy and Animation hurt 
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
    
    private IEnumerator TouchEnemyCoroutine()
    {
        isTouchEnemy = true;
        yield return new WaitForSeconds(touchEnemyDuration);
        isTouchEnemy = false;
    }
    #endregion

    public void Die()
    {
        m_healthBar.healthAmount = 100f;
        m_healthBar.healthBar.fillAmount = 1f;
        StartCoroutine(Respawn(0.5f));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(OverHeadCheck.position, OverHeadCheckRadius);
    }
}

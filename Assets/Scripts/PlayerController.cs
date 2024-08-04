using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 6f; // Character speed
    private float jumpPower = 13f;// Character jump

    private float horizontal;

    private bool isTouchGround;

    public Transform GroundCheck;
    public float GroundCheckRadius;
    public LayerMask groundLayer;

    [SerializeField] private Rigidbody2D rb;

    private Animator playerAnimation;

    Vector2 checkPointPos;

    GameController gameController;

    HealthManager m_healthBar;

    private GameObject currentTeleporter;

    // Start is called before the first frame update
    void Start()
    {
        checkPointPos = transform.position;

        rb = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
        m_healthBar = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();

        isTouchGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, groundLayer); // isTouchGround

        if (Input.GetButtonDown("Jump") && isTouchGround) // Jump
        {
            rb.velocity = Vector2.up * jumpPower;
            isTouchGround = false;
        }

        if (m_healthBar.healthAmount == 0) // Gameover
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTeleporter != null)
            {
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        playerAnimation.SetBool("OnGround", isTouchGround);
        playerAnimation.SetFloat("yVelocity", rb.velocity.y);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if (horizontal > 0f)
            transform.localScale = new Vector2(5, 5);
        else if (horizontal < 0f)
            transform.localScale = new Vector2(-5, 5);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_healthBar.takeDamage(10);
            rb.velocity = new Vector2(rb.velocity.x - 3f, rb.velocity.y);
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
    public void UpdateCheckPoint(Vector2 pos)
    {
        checkPointPos = pos;
    }
    IEnumerator Respawn(float duration) // Checkpoint
    {
        rb.velocity = new Vector2(0, 0);
        rb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = checkPointPos;
        transform.localScale = new Vector3(1, 1, 1);
        rb.simulated = true;
    }
    public void Die()
    {
        m_healthBar.healthAmount = 100f;
        m_healthBar.healthBar.fillAmount = 1f;
        StartCoroutine(Respawn(0.5f));
    }
}

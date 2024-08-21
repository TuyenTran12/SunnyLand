using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    private Animator animator; // Thêm biến animator

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }

        // Cập nhật animator
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void UpdateAnimator()
    {
        // Cập nhật trạng thái leo thang trong animator
        animator.SetBool("IsClimbing", isClimbing);

        // Cập nhật tốc độ leo thang (nếu cần)
        animator.SetFloat("ClimbSpeed", Mathf.Abs(vertical));
    }
}
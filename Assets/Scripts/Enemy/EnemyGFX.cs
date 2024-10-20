using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
    public AIPath aiPath;
    public AIDestinationSetter destinationSetter;
    public Transform player;

    private bool playerInRange = false;

    Animator animator;
    public bool IsDead = false;

    private float bounce = 5;
    public Rigidbody2D rb;

    Collider2D standingCollider;

    //Aduio game
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        // Tắt AI path finding ban đầu
        aiPath.enabled = false;

        standingCollider = GetComponent<PolygonCollider2D>();

        animator = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Xoay enemy
        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (playerInRange)
        {
            // Chỉ cập nhật đích đến khi player trong tầm
            destinationSetter.target = player;
            aiPath.enabled = true;
        }
        else
        {
            // Dừng di chuyển khi player không trong tầm
            aiPath.enabled = false;
        }
        animator.SetBool("IsDead", IsDead);
    }
    // Gọi khi player vào vùng phát hiện
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if(other.CompareTag("Hit"))
        {
            if (standingCollider.IsTouching(other))
            {
                // Destroy enemy object
                StartCoroutine(EnemyDeathSequence());

                // Apply bounce to player
                rb.velocity = new Vector2(rb.velocity.x, bounce);
            }
        }
    }

    // Gọi khi player rời khỏi vùng phát hiện
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator EnemyDeathSequence()
    {
        IsDead = true;
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        audioManager.PlaySFX(audioManager.EnemyDeath);
    }
}

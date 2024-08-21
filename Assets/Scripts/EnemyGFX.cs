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
    // Start is called before the first frame update
    void Start()
    {
        // Tắt AI path finding ban đầu
        aiPath.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Xoay enemy
        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-5, 5, 1);
        }
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localScale = new Vector3(5, 5, 1);
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
    }
    // Gọi khi player vào vùng phát hiện
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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
}

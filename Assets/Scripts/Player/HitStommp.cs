using System.Collections;
using UnityEngine;

public class HitStommp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D rb;

    EnemyGFX enemy;
    private void Start()
    {
        enemy = FindObjectOfType<EnemyGFX>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.isTrigger)
        {
            // Destroy enemy object
            StartCoroutine(EnemyDeathSequence());

            // Apply bounce to player
            rb.velocity = new Vector2(rb.velocity.x, bounce);
            
        }
    }
    private IEnumerator EnemyDeathSequence()
    {
        enemy.IsDead = true;
        yield return new WaitForSeconds(2);
        Destroy(enemy);
    }
}
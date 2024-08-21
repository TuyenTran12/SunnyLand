using UnityEngine;

public class HitStommp : MonoBehaviour
{
    public float bounce;
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !collision.isTrigger)
        {
            // Destroy enemy object
            Destroy(collision.gameObject);

            // Apply bounce to player
            rb.velocity = new Vector2(rb.velocity.x, bounce);
        }
    }
}
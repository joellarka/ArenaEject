using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyDelay = 3f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;
    public float frictionCoefficient = 10f; // Adjust this value to control the friction effect

    private bool isKnockbackApplied = false;
    private Rigidbody playerRb; // Reference to the player's rigidbody
    private PlayerFriction friction;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        if (!GetComponent<Collider>().isTrigger)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerRb == null)
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 knockbackDirection = -collision.contacts[0].normal;
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            friction = collision.gameObject.GetComponent<PlayerFriction>();
        }
    }

    void FixedUpdate()
    {
        if(playerRb != null)
        {
            if(friction != null && playerRb.velocity.magnitude > 0)
            {
                friction.DeacceleratePlayer();
            }
        }
    }
}
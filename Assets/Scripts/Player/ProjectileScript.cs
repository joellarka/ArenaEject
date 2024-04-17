using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyDelay = 3f;
    public float knockbackForce = 15f;
    public float knockbackDuration = 0.5f;

    private Rigidbody playerRb; // Reference to the player's rigidbody

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
            Vector3 knockbackDirection = collision.contacts[0].normal;
            playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
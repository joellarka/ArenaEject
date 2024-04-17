using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 25f;
    public float fireRate = 0.5f;
    public float spawnDistance = 1.0f; // Adjust this distance as needed
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (Input.GetButtonDown("P1_Fire_Duo") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Calculate spawn position in front of the player
        Vector3 spawnPosition = shootPoint.position + shootPoint.forward * spawnDistance;
        spawnPosition.y += 1;

        // Calculate spawn rotation with +90 degrees offset
        Quaternion spawnRotation = shootPoint.rotation * Quaternion.Euler(0, 0, 0);

        // Instantiate the projectile at the calculated position and rotation
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, spawnRotation);

        // Get the Rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Apply force to the projectile
        if (rb != null)
        {
            rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        }
    }
}
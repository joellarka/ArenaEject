using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Ammo ammoTypePrefab;
    public Transform firePoint;
    public int ammoCount = 1;
    public float initialUpwardForce = 20f;
    [SerializeField] protected float fireRate = 0.5f;
    private float fireTimer = 0;
    [SerializeField] protected bool weaponDeterminesAmmoSpeed = true;
    [SerializeField] protected float ammoSpeed = 3f;
    [SerializeField] protected Vector3 ammoDirOffSet = Vector3.zero;

    public virtual bool TryShoot()
    {
        if (ammoCount <= 0) return false;
        if (ammoTypePrefab == null) return false;
        if (firePoint == null) return false;
        if (Time.time <= fireTimer) return false;
        
        Shoot();
        return true;
    }

    protected virtual void Shoot()
    {
        fireTimer = Time.time + fireRate;
        ammoCount--;

        GameObject projectileObj = Instantiate(ammoTypePrefab, firePoint.position, Quaternion.identity).gameObject;
        projectileObj.transform.forward = transform.forward;

        Ammo projectileScr = projectileObj.GetComponent<Ammo>();
        Vector3 projectileDir = projectileObj.transform.forward;
        ammoDirOffSet.Normalize();
        projectileDir.Normalize();
        projectileScr.moveDir = projectileDir;

        if (!projectileObj.GetComponent<Projectile>())
        {
            Rigidbody projectileRb = projectileObj.GetComponent<Rigidbody>();
            projectileRb.velocity = Vector3.zero;
            projectileRb.AddForce(Vector3.up * initialUpwardForce, ForceMode.Impulse);
        }

        if (weaponDeterminesAmmoSpeed)
        {
            projectileScr.moveSpeed = ammoSpeed;
        }

    }
}

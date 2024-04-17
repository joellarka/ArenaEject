using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // TODO:
    // Model

    public Ammo ammoTypePrefab;
    public Transform firePoint;
    [SerializeField] protected int ammoCount = 1;
    [SerializeField] protected float fireRate = 0.5f;
    [SerializeField] protected bool weaponDeterminesAmmoSpeed = true;
    [SerializeField] protected float ammoSpeed = 3f;

    public virtual bool TryShoot()
    {
        if (ammoCount <= 0) return false;
        if (ammoTypePrefab == null) return false;
        if (firePoint == null) return false;
        if (Time.time <= fireRate) return false;
        
        Shoot();
        return true;
    }

    protected virtual void Shoot()
    {
        fireRate = Time.time + fireRate;
        ammoCount--;

        GameObject projectileObj = Instantiate(ammoTypePrefab, firePoint.position, Quaternion.identity).gameObject;
        projectileObj.transform.forward = transform.forward;

        Ammo projectileScr = projectileObj.GetComponent<Ammo>();
        projectileScr.moveDir = projectileObj.transform.forward;
        if (weaponDeterminesAmmoSpeed)
        {
            projectileScr.moveSpeed = ammoSpeed;
        }

    }
}

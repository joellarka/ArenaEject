using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float grenadeLaunchForce = 5f;
    public float grenadeUpwardForce = 5f;

    private Rigidbody rb;
    private MonoBehaviour explosion;

    void Start()
    {
        explosion = GetComponent<Explosion>();

        if(explosion != null)
        {
            explosion.enabled = false;
        }
        else
        {
            Debug.Log("NULLLLL");
        }

        rb = GetComponent<Rigidbody>();
        FreezeYPosition();
    }

    void Update()
    {
        if (CheckGrenade())
        {
            //WEAPON DROP
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropWeapon();
            }
        }
    }

    void FreezeYPosition()
    {
        // Set constraints to freeze position in the Y axis only
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    void UnfreezeYPosition()
    {
        // Remove constraints to allow movement in the Y axis
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }

    private bool CheckGrenade()
    {
        WeaponUser weaponUser = GetComponentInParent<WeaponUser>();

        if(weaponUser != null)
        {
            return true;
        }
        return false;
    }

    public void DropWeapon()
    {
        WeaponUser weaponUser = GetComponentInParent<WeaponUser>();

        if (weaponUser.inventory.Count > 0)
        {
            Weapon lastWeapon = weaponUser.inventory[weaponUser.inventory.Count - 1];
            Vector3 dropPosition = transform.position + transform.forward * 1f;

            lastWeapon.gameObject.SetActive(true);
            lastWeapon.transform.position = dropPosition;
            UnfreezeYPosition();

            Rigidbody grenadeRb = lastWeapon.GetComponent<Rigidbody>();
            if(grenadeRb != null)
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Vector3 launchDirection = weaponUser.transform.forward;

                grenadeRb.AddForce(launchDirection * grenadeLaunchForce, ForceMode.Impulse);
                grenadeRb.AddForce(transform.TransformDirection(Vector3.up) * grenadeUpwardForce, ForceMode.Impulse);
                gameObject.transform.SetParent(null);
                collider.isTrigger = false;
                explosion.enabled = true;
            }

            weaponUser.inventory.Remove(lastWeapon);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            Destroy(gameObject);
        }
    }
}

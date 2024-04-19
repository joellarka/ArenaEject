using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUser : MonoBehaviour
{
    [HideInInspector] public int controllerIndex = 1;
    [HideInInspector] public bool appropriatlySpawned = false;

    public Transform carriedWeaponTransform;

    public List<Weapon> inventory = new List<Weapon>();
    private bool isNearWeapon = false;
    public Weapon carriedWeapon;
    private Weapon nearbyWeapon;
    private Vector3 carriedWeaponOffset;
    private float nextFireTime = 0f;
    public float fireRate = 0.5f;

    void Update()
    {
        /*if (Input.GetAxisRaw($"P{controllerIndex}_Fire_Duo") > 0.5f)
        {
            TryFireWeapon();
            nextFireTime = Time.time + fireRate;
        }*/

        if(inventory.Count > 0)
        {
            UpdateCarriedWeaponPosition();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            isNearWeapon = true;
            nearbyWeapon = other.GetComponent<Weapon>();
            PickupWeapon();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            isNearWeapon = false;
            nearbyWeapon = null;
        }
    }

    private void TryFireWeapon()
    {
        if (carriedWeapon == null) return;

        bool result = carriedWeapon.TryShoot();
    }

    public void PickupWeapon()
    {
        if (nearbyWeapon != null)
        {
            inventory.Add(nearbyWeapon);
            nearbyWeapon.transform.SetParent(transform);
            nearbyWeapon.transform.localPosition = Vector3.zero;
            nearbyWeapon.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    private void UpdateCarriedWeaponPosition()
    {
        foreach (Weapon weapon in inventory)
        {
            carriedWeaponOffset = new Vector3(0.6f, 1f, 1);
            weapon.transform.position = carriedWeaponTransform.position + carriedWeaponOffset;
        }
    }
}
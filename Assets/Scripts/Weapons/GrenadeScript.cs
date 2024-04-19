using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    public float grenadeLaunchForce = 10f;
    public float grenadeUpwardForce = 5f;
    public float triggerThreshold = 0.5f;

    private Rigidbody rb;
    private MonoBehaviour explosion;
    private MeshRenderer rbMesh;
    private int collisionCounter = 0;
    [HideInInspector] public int controllerIndex = 1;

    bool IsTriggerPressed = false;

    void Start()
    {
        explosion = GetComponent<Explosion>();
        rbMesh = GetComponent<MeshRenderer>();

        if(explosion != null)
        {
            explosion.enabled = false;
        }
        else
        {
            Debug.Log("NULLLLL");
        }

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float triggerInput = Input.GetAxis($"P{controllerIndex}_Fire_Duo");

        if (CheckGrenade())
        {
            //WEAPON DROP
            if (Input.GetAxis($"P{controllerIndex}_Fire_Duo") > 0)
            {
                DropWeapon(); 
            }
        }

        Debug.Log("triggerInput: " + triggerInput);
    }

    void FreezeYPosition()
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;
    }

    void UnfreezeYPosition()
    {
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
            collisionCounter += 1;

            if(collisionCounter > 1)
            {
                rbMesh.enabled = false;
                StartCoroutine(DestroyAfterDuration());
                collisionCounter = 0;
            }
            else
            {
                FreezeYPosition();
                BoxCollider myCollider = GetComponent<BoxCollider>();
                myCollider.isTrigger = true;
            }
        }
    }

    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}

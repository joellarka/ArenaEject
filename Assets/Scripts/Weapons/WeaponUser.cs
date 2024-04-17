using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUser : MonoBehaviour
{
    [HideInInspector] public int controllerIndex = 1;
    [HideInInspector] public bool appropriatlySpawned = false;

    public Weapon carriedWeapon;

    private void Update()
    {
        if(Input.GetAxisRaw($"P{controllerIndex}_Fire_Duo") > 0.5f)
        {
            TryFireWeapon();
        }
    }

    private void TryFireWeapon()
    {
        Debug.Log("Attempt fire!");
    }
}

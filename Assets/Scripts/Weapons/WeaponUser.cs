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
        
    }

    private void TryFireWeapon()
    {

    }
}

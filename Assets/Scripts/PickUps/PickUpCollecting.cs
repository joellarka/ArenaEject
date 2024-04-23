using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCollecting : MonoBehaviour
{
    private WeaponUser weaponUser;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (!TryGetComponent<WeaponUser>(out weaponUser))
        {
            Debug.LogError("PickUpCollecting unable to find WeaponUser");
        }

        if (!TryGetComponent<PlayerStats>(out playerStats))
        {
            Debug.LogError("PickUpCollecting unable to find PlayerStats");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<PickUp>(out PickUp encounteredPickUp)) return;

        switch (encounteredPickUp.myType)
        {
            case PickUpType.WEAPON:
                {
                    if (weaponUser == null) break;
                    if (weaponUser.AttemptAquireWeapon(encounteredPickUp.weaponPrefab)) {
                        Destroy(encounteredPickUp.gameObject);
                    };
                    break;
                }
            case PickUpType.HEALTH:
                {

                    Debug.Log("HEALTH pickups not implemented");
                    break;
                }
            case PickUpType.MOVESPEED:
                {

                    Debug.Log("MOVESPEED pickups not implemented");
                    break;
                }
            default:
                {
                    Debug.LogError("Switch statement reached default, missing case?");
                    break;
                }
        }
    }
}
